using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading.Tasks;

public class AzureQueueFunction
{
    public static async Task WriteToQueue(string queueName, string message)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=abcretaildata;AccountKey=0WySTkSNJFOphLKnq9zzBpuulyy24FFYt1o8G4IdpsB2OQAf8GT+npRXiFJ4I6ebpMSwux7mSDfX+ASteh9OIQ==;EndpointSuffix=core.windows.net");
        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
        CloudQueue queue = queueClient.GetQueueReference(queueName);

        await queue.CreateIfNotExistsAsync();

        CloudQueueMessage queueMessage = new CloudQueueMessage(message);
        await queue.AddMessageAsync(queueMessage);
    }

    public static async Task<string> ReadFromQueue(string queueName)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=abcretaildata;AccountKey=0WySTkSNJFOphLKnq9zzBpuulyy24FFYt1o8G4IdpsB2OQAf8GT+npRXiFJ4I6ebpMSwux7mSDfX+ASteh9OIQ==;EndpointSuffix=core.windows.net");
        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
        CloudQueue queue = queueClient.GetQueueReference(queueName);

        await queue.CreateIfNotExistsAsync();

        CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();
        if (retrievedMessage != null)
        {
            await queue.DeleteMessageAsync(retrievedMessage);
            return retrievedMessage.AsString;
        }
        return null;
    }
}
