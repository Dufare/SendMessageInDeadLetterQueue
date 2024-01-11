using Azure.Messaging.ServiceBus;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
       
            static async Task Main(string[] args)
            {
                string connectionString = "Endpoint=sb://dev-bunzlecom-mdw-sbus.servicebus.windows.net/;SharedAccessKeyName=sendpolicy;SharedAccessKey=sFfdCKC93bvUPs9WHX8XH353sRdy7jqSXzSbm9n6NUg=;EntityPath=order-submit-bunzl-queue";
                string queueName = "order-submit-bunzl-queue";
                await SendMessageToDeadLetterQueue(connectionString, queueName,
                    "{\r\n  \"Id\": \"4023c912-bac1-4391-9703-b0d901a\",\r\n  \"ErpOrderNumber\": null,\r\n  \"CompanyId\": 76,\r\n  \"LocationId\": \"430\",\r\n  \"ShippingAddress\": {\r\n    \"Name\": \"SLONE JANITORIAL DBA KING SUPPLY\",\r\n    \"AddressLine1\": \"8274 KENTUCKY RT 1428\",\r\n    \"AddressLine2\": \"\",\r\n    \"Unit\": null,\r\n    \"City\": \"ALLEN\",\r\n    \"State\": \"Kentucky\",\r\n    \"ZipCode\": \"41601\",\r\n    \"Country\": \"United States\"\r\n  },\r\n  \"BillingAddress\": {\r\n    \"Name\": \"SLONE JANITORIAL DBA KING SUPPLY\",\r\n    \"AddressLine1\": \"8274 KENTUCKY ROUTE 1428\",\r\n    \"AddressLine2\": \"\",\r\n    \"Unit\": \"\",\r\n    \"City\": \"ALLEN\",\r\n    \"State\": \"Kentucky\",\r\n    \"ZipCode\": \"41601\",\r\n    \"Country\": \"United States\"\r\n  },\r\n  \"OrderDate\": \"2023-12-15T14:52:31.013677\",\r\n  \"BillToNumber\": \"305004\",\r\n  \"ShipToNumber\": \"BAKE\",\r\n  \"Department\": \"\",\r\n  \"OrderNumber\": \"WEB3528797\",\r\n  \"Notes\": \"\",\r\n  \"CarrierCode\": \"BNZD\",\r\n  \"Status\": \"Submitted\",\r\n  \"PONumber\": \"1114609\",\r\n  \"PromotionCode\": null,\r\n  \"InitiatedbyUserName\": \"430970002DS\",\r\n  \"TotalQtyOrdered\": 35,\r\n  \"LineCount\": 8,\r\n  \"SalesPersonName\": \"\",\r\n  \"RequestedDeliveryDate\": \"2023-12-21T00:00:00-05:00\",\r\n  \"Terms\": null,\r\n  \"ShipCode\": null,\r\n  \"ProductTotal\": 0.0,\r\n  \"OtherCharges\": 0.0,\r\n  \"OrderDiscountAmount\": 0.0,\r\n  \"ProductDiscountAmount\": 0.0,\r\n  \"HandlingCharges\": 0.0,\r\n  \"OrderGrandTotal\": 0.0,\r\n  \"ShippingCharges\": 0.0,\r\n  \"CurrencyCode\": null,\r\n  \"OrderShipment\": null,\r\n  \"TaxAmount\": 0.0,\r\n  \"Payment\": {\r\n    \"Terms\": \"P\",\r\n    \"CardType\": null,\r\n    \"ExpirationMonth\": 0,\r\n    \"ExpirationYear\": 0,\r\n    \"CardHolderName\": null,\r\n    \"Avs\": null,\r\n    \"TransactionID\": null,\r\n    \"TransactionTag\": null,\r\n    \"TokenValue\": null,\r\n    \"ExpirationDate\": null,\r\n    \"Amount\": null,\r\n    \"DateAuthorization\": null,\r\n    \"TimeAuthorization\": null\r\n  },\r\n  \"LastModifiedDate\": \"0001-01-01T00:00:00\",\r\n  \"CartLines\": [\r\n    {\r\n      \"Line\": 1,\r\n      \"ERPNumber\": \"13709461\",\r\n      \"QtyOrdered\": 3,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 2,\r\n      \"ERPNumber\": \"14500040\",\r\n      \"QtyOrdered\": 5,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 3,\r\n      \"ERPNumber\": \"11900181\",\r\n      \"QtyOrdered\": 5,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 4,\r\n      \"ERPNumber\": \"75000764\",\r\n      \"QtyOrdered\": 4,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 5,\r\n      \"ERPNumber\": \"75000765\",\r\n      \"QtyOrdered\": 8,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 6,\r\n      \"ERPNumber\": \"12500152\",\r\n      \"QtyOrdered\": 2,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 7,\r\n      \"ERPNumber\": \"38385418\",\r\n      \"QtyOrdered\": 6,\r\n      \"SalePrice\": null\r\n    },\r\n    {\r\n      \"Line\": 8,\r\n      \"ERPNumber\": \"12500220\",\r\n      \"QtyOrdered\": 2,\r\n      \"SalePrice\": null\r\n    }\r\n  ],\r\n  \"CustomerNumber\": null,\r\n  \"OrderLines\": null\r\n}",
                    "Customer Account 970002 Status Is DNS");
            }


            static async Task SendMessageToDeadLetterQueue(string connectionString, string queueName, string messageContent, string errorDescription)
            {
                await using (ServiceBusClient client = new ServiceBusClient(connectionString))
                {
                    ServiceBusSender sender = client.CreateSender(queueName);
                    ServiceBusMessage message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageContent));
                    message.ApplicationProperties["DeadLetterReason"] = "SimpleHttpResponseException";
                    message.ApplicationProperties["DeadLetterErrorDescription"] = errorDescription;

                    try
                    {
                        await sender.SendMessageAsync(message);
                        Console.WriteLine("Message sent to the dead-letter queue with error description.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        
    }
}