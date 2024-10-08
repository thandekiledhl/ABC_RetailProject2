using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Threading.Tasks;

public class AzureBlobStorageFunction
{
    public static async Task WriteToBlobStorage(string containerName, string blobName, string filePath)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=abcretaildata;AccountKey=0WySTkSNJFOphLKnq9zzBpuulyy24FFYt1o8G4IdpsB2OQAf8GT+npRXiFJ4I6ebpMSwux7mSDfX+ASteh9OIQ==;EndpointSuffix=core.windows.net");
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        CloudBlobContainer container = blobClient.GetContainerReference(containerName);

        await container.CreateIfNotExistsAsync();

        CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
        using (var fileStream = File.OpenRead(filePath))
        {
            await blockBlob.UploadFromStreamAsync(fileStream);
        }
    }
}

