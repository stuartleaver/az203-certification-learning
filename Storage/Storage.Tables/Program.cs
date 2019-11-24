using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Storage.Tables
{
    class Program
    {
        private static readonly string storageAccountConnectionString = "<connection-string>";
        
        private static CloudStorageAccount cloudStorageAccount;

        private static CloudTableClient tableClient;

        private static CloudTable table;

        private static string tableName;

        static async Task Main(string[] args)
        {
            cloudStorageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);

            tableClient = cloudStorageAccount.CreateCloudTableClient();
            
            await SimpleInsertAndQueryExample();

            Console.WriteLine("Press any key to continue");

            await BatchExecuteExample();

            Console.WriteLine("Press any key to exit");

            Console.ReadLine();
        }

        private static async Task SimpleInsertAndQueryExample()
        {
            Console.WriteLine("Executing simple insert and query example");
            Console.WriteLine("=========================================");

            tableName = $"demoSimple{Guid.NewGuid().ToString().Substring(0, 5)}";

            table = await CreateTableAsync(tableName);

            await InsertTableRow("Steve", "Smith");
            await InsertTableRow("Lewis", "Hamilton");
            await InsertTableRow("Lionel", "Messi");

            await QueryEmployeesTable();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("=================================");
            Console.WriteLine(Environment.NewLine);

        }

        private static async Task BatchExecuteExample()
        {
            Console.WriteLine("Executing batch operation example");
            Console.WriteLine("=================================");

            tableName = $"demoBatch{Guid.NewGuid().ToString().Substring(0, 5)}";

            table = await CreateTableAsync(tableName);

            var batchOperation = new TableBatchOperation();

            for(var i = 1; i <= 100; i++)
            {
                batchOperation.InsertOrMerge(new EmployeeEntity($"FirstName-{i}", $"LastName-{i}"));
            }

            var results = await table.ExecuteBatchAsync(batchOperation);

            foreach (var res in results)
            {
                var entity = res.Result as EmployeeEntity;

                Console.WriteLine($"Inserted entity with ETag = {entity.ETag} and RowKey = {entity.RowKey}");
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("=================================");
            Console.WriteLine(Environment.NewLine);
        }

        private static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            var table = tableClient.GetTableReference(tableName);

            try
            {
                if(await table.CreateIfNotExistsAsync())
                {
                    Console.WriteLine($"Created table with name: {tableName}.");
                }
                else
                {
                    Console.WriteLine($"Table {tableName} already exists.");
                }
            }
            catch(StorageException ex)
            {
                Console.WriteLine($"An error occured creating table {tableName}. Error - {ex.InnerException}");

                throw;
            }

            return table;
        }

        private static async Task InsertTableRow(string firstName, string lastName)
        {
            var employeeEntity = new EmployeeEntity(firstName, lastName);

            var insertOperation = TableOperation.Insert(employeeEntity);

            await table.ExecuteAsync(insertOperation);
        }

        private static async Task QueryEmployeesTable()
        {
            TableContinuationToken token = null;

            var tableQuery = new TableQuery<EmployeeEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "staff"));

            tableQuery.TakeCount = 50;

            var segment = 1;

            do
            {
                var employees = await table.ExecuteQuerySegmentedAsync(tableQuery, token);

                token = employees.ContinuationToken;

                foreach(EmployeeEntity employee in employees)
                {
                    Console.WriteLine($"Segment {segment}: {employee.RowKey}");
                }

                segment++;
            } while (token != null);
        }

        private static async Task EmptyEmployeeTable()
        {
            var tableQuery = new TableQuery<EmployeeEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "staff"));

            var employees = await table.ExecuteQuerySegmentedAsync(tableQuery, new TableContinuationToken());

            foreach (var employee in employees)
            {
                var tableOperation = TableOperation.Delete(employee);

                await table.ExecuteAsync(tableOperation);
            }
        }
    }
}
