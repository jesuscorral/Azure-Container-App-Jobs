using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Features.InstallService;

// the client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

// the sender used to publish messages to the queue
ServiceBusSender sender;

// number of messages to be sent to the queue
const int numOfMessages = 2;

var clientOptions = new ServiceBusClientOptions()
{ 
    TransportType = ServiceBusTransportType.AmqpWebSockets
};
// TODO: set connection string to your Service Bus namespace
var namespaceConnectionString = "<<Connection_String_Service_Bus_NameSpace>>"; 
// TODO: set the name of your Service Bus queue
var queueName = "<<Service Bus Queue>>";

client = new ServiceBusClient(namespaceConnectionString, clientOptions);
sender = client.CreateSender(queueName);

// create a batch 
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= numOfMessages; i++)
{
    // try adding a message to the batch
   
    var installServiceDto = new InstallServiceDto() { ServiceKey = "Sidra", Parameters = new Dictionary<string, object>() { { "test", "test" } } };
    var messageBody = JsonSerializer.Serialize(installServiceDto);

     if (!messageBatch.TryAddMessage(new ServiceBusMessage(messageBody)))
    {
        // if it is too large for the batch
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    // Use the producer client to send the batch of messages to the Service Bus queue
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Press any key to end the application");
