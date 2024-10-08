using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
using System.Threading.Tasks;

public class AzureFileFunction
{
    public static async Task WriteToFileStorage(string shareName, string fileName, string filePath)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=abcretaildata;AccountKey=0WySTkSNJFOphLKnq9zzBpuulyy24FFYt1o8G4IdpsB2OQAf8GT+npRXiFJ4I6ebpMSwux7mSDfX+ASteh9OIQ==;EndpointSuffix=core.windows.net");
        CloudFileClient fileClient = storageAccount.CreateCloudFileClient();
        CloudFileShare fileShare = fileClient.GetShareReference(shareName);

        await fileShare.CreateIfNotExistsAsync();

        CloudFileDirectory rootDirectory = fileShare.GetRootDirectoryReference();
        CloudFile file = rootDirectory.GetFileReference(fileName);

        using (var fileStream = File.OpenRead(filePath))
        {
            await file.UploadFromStreamAsync(fileStream);
        }
    }
}

