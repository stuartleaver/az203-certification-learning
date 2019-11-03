using Microsoft.Azure.Cosmos;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDB.Optimisation
{
    class Program
    {
        // The Azure Cosmos DB endpoint for running this sample.
        private static readonly string EndpointUri = "<your endpoint here>";

        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "<your primary key>";

        // The Cosmos client instance
        private static CosmosClient cosmosClient;

        // The database we will create
        private static Database database;

        // The container we will create
        private static Container container;

        // The name of the database and container we will create
        private static readonly string databaseId = "az203";

        private static readonly string containerId = "customers";

        static async Task Main()
        {
            Console.WriteLine("Beginning demo...");

            await SetupCosmosDbAsync();

            await RunDemo();
        }

        public static async Task SetupCosmosDbAsync()
        {
            var cosmosClientOptions = new CosmosClientOptions()
            {
                MaxRetryAttemptsOnRateLimitedRequests = 9
            };

            // Create the Cosmon client
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, cosmosClientOptions);

            await CreateCosmosDatabaseAsync();

            await CreateCosmosDatabaseContainerAsync();
        }

        public static async Task RunDemo()
        {
            // The number of customers to create
            var customerCount = 50;

            var customerFactory = new CustomerFactory();

            var timer = new Stopwatch();

            // Get the container on which we will be creating documents
            container = cosmosClient.GetContainer(databaseId, containerId);

            while (true)
            {
                // Generate a randon list of Customers
                var customers = customerFactory.Generate(customerCount);

                timer.Start();

                // Create a document in the selected container for each Customer using the given partician key
                var response = await Task.WhenAll(customers.Select(i => container.CreateItemAsync(i, new PartitionKey(i.FirstName))));

                timer.Stop();

                Console.WriteLine($"Created {customers.Count()} Customers - Cost was {response.Sum(i => i.RequestCharge)} RU's and an execution time of {timer.ElapsedMilliseconds}ms");

                timer.Reset();
            }
        }

        private static async Task CreateCosmosDatabaseAsync()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            Console.WriteLine($"Created database - {database.Id}");
        }

        private static async Task CreateCosmosDatabaseContainerAsync()
        {
            container = await database.CreateContainerIfNotExistsAsync(containerId, "/FirstName");

            Console.WriteLine($"Created container - {container.Id}");
        }
    }
}