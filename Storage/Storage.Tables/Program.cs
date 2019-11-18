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

        private static CloudTable employeeTable;

        static async Task Main(string[] args)
        {
            cloudStorageAccount = CloudStorageAccount.Parse(storageAccountConnectionString);

            tableClient = cloudStorageAccount.CreateCloudTableClient();

            employeeTable = tableClient.GetTableReference("Employees");

            await EmptyEmployeeTable();
            
            await InsertTableRow("Steve", "Smith");
            await InsertTableRow("Lewis", "Hamilton");
            await InsertTableRow("Lionel", "Messi");

            await QueryEmployeesTable();

            Console.ReadLine();
        }

        private static async Task InsertTableRow(string firstName, string lastName)
        {
            var employeeEntity = new EmployeeEntity(firstName, lastName);

            var insertOperation = TableOperation.Insert(employeeEntity);

            await employeeTable.ExecuteAsync(insertOperation);
        }

        private static async Task QueryEmployeesTable()
        {
            TableContinuationToken token = null;

            var tableQuery = new TableQuery<EmployeeEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "staff"));

            tableQuery.TakeCount = 50;

            var segment = 1;

            do
            {
                var employees = await employeeTable.ExecuteQuerySegmentedAsync(tableQuery, token);

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

            var employees = await employeeTable.ExecuteQuerySegmentedAsync(tableQuery, new TableContinuationToken());

            foreach (var employee in employees)
            {
                var tableOperation = TableOperation.Delete(employee);

                await employeeTable.ExecuteAsync(tableOperation);
            }
        }
    }
}
