using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Features.InstallService;

var cg = new ConfigurationBuilder().AddEnvironmentVariables().Build();
var queueName = cg["queueName"];
var namespaceConnectionString = cg["namespaceConnectionString"];


ServiceBusClient client;
ServiceBusProcessor processor;

Console.WriteLine("Starting!!!");

var clientOptions = new ServiceBusClientOptions()
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};

client = new ServiceBusClient(namespaceConnectionString, clientOptions);

Console.WriteLine("Creating receiver");

ServiceBusReceiver receiver = client.CreateReceiver(queueName);

IReadOnlyList<ServiceBusReceivedMessage> receivedMessages = await receiver.ReceiveMessagesAsync(maxMessages: 10);

foreach (ServiceBusReceivedMessage receivedMessage in receivedMessages)
{
    // get the message body as a string
    string body = receivedMessage.Body.ToString();
    var deserializedMessage = JsonSerializer.Deserialize<InstallServiceDto>(body);

    Console.WriteLine($"Deserialized: {deserializedMessage?.ServiceKey}");

    Console.WriteLine($"Received: {body}");

    // complete the message. message is deleted from the queue. 
    await receiver.CompleteMessageAsync(receivedMessage);
}
