using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerFunc.Azure.Storage.Table.Read.Client
{
    public class Program
    {
        /// <summary>
        /// The table
        /// </summary>
        private static CloudTable table;

        private static IConfigurationRoot config;
        static async Task Main(string[] args)
        {
            config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false).AddEnvironmentVariables().Build();
            ConnectToCloudTable();
            string partitionKey = "Client1";
            string rowkey = "1";
            await RetrieveRecord(table, partitionKey, rowkey);

            var customerProfiles = await GetAllEntitiesFromTable<CustomerProfile>(table);
            foreach (var customer in customerProfiles)
            {
                Console.WriteLine($"{nameof(customer.customerID)} {customer.RowKey}");
                Console.WriteLine($"{nameof(customer.CustomerType)} {customer.PartitionKey}");
                Console.WriteLine($"{nameof(customer.FirstName)} {customer.FirstName}");
                Console.WriteLine($"{nameof(customer.MiddleName)} {customer.MiddleName}");
                Console.WriteLine($"{nameof(customer.LastName)} {customer.LastName}");
                Console.WriteLine($"{nameof(customer.MobileNumber)} {customer.MobileNumber}");
                Console.WriteLine("----------------------------------------------------------------");
            }
        }

        private static void ConnectToCloudTable()
        {
            var connectionString = config["StorageAccountConnectionString"];
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(config["TableName"]);
        }

        public async static Task RetrieveRecord(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<CustomerProfile>(partitionKey, rowKey);
            TableResult tableResult = await table.ExecuteAsync(tableOperation);
            var profile = tableResult.Result as CustomerProfile;

            if (profile != null)
            {
                Console.WriteLine($"{nameof(profile.customerID)} {profile.RowKey}");
                Console.WriteLine($"{nameof(profile.CustomerType)} {profile.PartitionKey}");
                Console.WriteLine($"{nameof(profile.FirstName)} {profile.FirstName}");
                Console.WriteLine($"{nameof(profile.MiddleName)} {profile.MiddleName}");
                Console.WriteLine($"{nameof(profile.LastName)} {profile.LastName}");
                Console.WriteLine($"{nameof(profile.MobileNumber)} {profile.MobileNumber}");
                Console.WriteLine("----------------------------------------------------------------");
            }
        }

        private static async Task<IEnumerable<T>> GetAllEntitiesFromTable<T>(CloudTable table) where T : ITableEntity, new()
        {
            TableQuerySegment<T> querySegment = null;
            var entities = new List<T>();
            var query = new TableQuery<T>();

            do
            {
                querySegment = await table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                entities.AddRange(querySegment.Results);
            } while (querySegment.ContinuationToken != null);

            return entities;
        }
    }
}
