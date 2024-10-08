using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

public class AzureTableStorageFunction
{
    public static async Task StoreDataInAzureTable(string tableName, string partitionKey, string rowKey, string customerInfo)
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=abcretaildata;AccountKey=0WySTkSNJFOphLKnq9zzBpuulyy24FFYt1o8G4IdpsB2OQAf8GT+npRXiFJ4I6ebpMSwux7mSDfX+ASteh9OIQ==;EndpointSuffix=core.windows.net");
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
        CloudTable table = tableClient.GetTableReference(tableName);

        await table.CreateIfNotExistsAsync();

        CustomerEntity entity = new CustomerEntity(partitionKey, rowKey)
        {
            CustomerInfo = customerInfo
        };

        TableOperation insertOperation = TableOperation.InsertOrMerge(entity);
        await table.ExecuteAsync(insertOperation);
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;

        }

        public string CustomerInfo { get; set; }
    }
}


