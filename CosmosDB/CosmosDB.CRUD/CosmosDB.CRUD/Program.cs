using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosDB.CRUD
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
        private static readonly string databaseId = "az203-crud";

        private static readonly string containerId = "families";

        // Family factory
        private static FamilyGenerator familyGenerator;

        static async Task Main()
        {
            Console.WriteLine("Beginning demo...");

            await SetupCosmosDbAsync();

            familyGenerator = new FamilyGenerator();

            try
            {
                await CountDocumentsInContainer("SELECT VALUE COUNT(1) FROM c");

                await CreateDocument();

                await CreateDocumentWhenItemNotPresent();

                await QueryForDocuments();

                await UpdateDocument();

                await DeleteDocument();

                await DeleteDatabase();

                Console.WriteLine("End of demo, press any key to quit.");

                Console.ReadLine();
            }
            catch(CosmosException ex)
            {
                Console.WriteLine($"Cosmos Exception: {ex}");
            }
            finally
            {
                cosmosClient.Dispose();
            }
        }

        public static async Task SetupCosmosDbAsync()
        {
            var cosmosClientOptions = new CosmosClientOptions()
            {
                MaxRetryAttemptsOnRateLimitedRequests = 9
            };

            // Create the Cosmos client
            cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, cosmosClientOptions);

            await CreateCosmosDatabaseAsync();

            await CreateCosmosDatabaseContainerAsync();
        }

        public static async Task CountDocumentsInContainer(string sql)
        {
            var queryDefinition = new QueryDefinition(sql);

            var queryResultSetIterator = container.GetItemQueryIterator<dynamic>(queryDefinition);

            while(queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<dynamic> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach(var row in currentResultSet)
                {
                    Console.WriteLine($"The number of rows in the container {container.Id} for the given query is {row}");
                }
            }
        }

        public static async Task CreateDocument()
        {
            var family = familyGenerator.GenerateAndersenFamily();

            try
            {
                ItemResponse<Family> response = await container.CreateItemAsync(family, new PartitionKey(family.LastName));

                Console.WriteLine($"Created family {family.LastName} at a cost of {response.RequestCharge} RUs");
            }
            catch(CosmosException ex)
            {
                Console.WriteLine($"Cosmos Exception: {ex}");
            }
        }

        public static async Task CreateDocumentWhenItemNotPresent()
        {
            var family = familyGenerator.GenerateWakefieldFamily();

            try
            {
                ItemResponse<Family> response = await container.ReadItemAsync<Family>(family.Id, new PartitionKey(family.LastName));

                Console.WriteLine($"Item in database with id: {response.Resource.Id} already exists");
            }
            catch(CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                var response = await container.CreateItemAsync(family, new PartitionKey(family.LastName));

                Console.WriteLine($"Created family with Id {response.Resource.Id} at a cost of {response.RequestCharge} RUs");
            }
        }

        public static async Task QueryForDocuments()
        {
            var sql = "SELECT * FROM c";

            var queryDefinition = new QueryDefinition(sql);

            var queryResultSetIterator = container.GetItemQueryIterator<Family>(queryDefinition);

            while(queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Family> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach(Family family in currentResultSet)
                {
                    Console.WriteLine($"Family Id {family.Id} found");
                }
            }
        }

        public static async Task UpdateDocument()
        {
            var family = familyGenerator.GenerateWakefieldFamily();

            var sql = "SELECT VALUE COUNT(1) FROM c WHERE c.IsRegistered = true";

            await CountDocumentsInContainer(sql);

            family.IsRegistered = false;

            ItemResponse<Family> response = await container.ReplaceItemAsync(family, family.Id, new PartitionKey(family.LastName));

            Console.WriteLine($"Updated {response.Resource.LastName} family at a cost of {response.RequestCharge} RUs");

            await CountDocumentsInContainer(sql);
        }

        public static async Task DeleteDocument()
        {
            var family = familyGenerator.GenerateAndersenFamily();

            var sql = "SELECT VALUE COUNT(1) FROM c WHERE c.LastName = 'Andersen'";

            await CountDocumentsInContainer(sql);

            ItemResponse<Family> response = await container.DeleteItemAsync<Family>(family.Id, new PartitionKey(family.LastName));

            Console.WriteLine($"Deleted {family.LastName} family at a cost of {response.RequestCharge} RUs");

            await CountDocumentsInContainer(sql);
        }

        public static async Task DeleteDatabase()
        {
            var response = await database.DeleteAsync();

            Console.WriteLine($"Deleted database {databaseId} at a cost of {response.RequestCharge} RUs");

            cosmosClient.Dispose();
        }

        private static async Task CreateCosmosDatabaseAsync()
        {
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            Console.WriteLine($"Created database - {database.Id}");
        }

        private static async Task CreateCosmosDatabaseContainerAsync()
        {
            container = await database.CreateContainerIfNotExistsAsync(containerId, "/LastName");

            Console.WriteLine($"Created container - {container.Id}");
        }
    }
}