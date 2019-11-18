using Microsoft.WindowsAzure.Storage.Table;

namespace Storage.Tables
{
    public class EmployeeEntity : TableEntity
    {
        public EmployeeEntity() { }
        
        public EmployeeEntity(string firstName, string LastName)
        {
            PartitionKey = "staff";

            RowKey = string.Concat(firstName, " ", LastName);
        }
    }
}