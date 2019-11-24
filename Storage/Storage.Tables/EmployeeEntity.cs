using Microsoft.WindowsAzure.Storage.Table;

namespace Storage.Tables
{
    public class EmployeeEntity : TableEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public EmployeeEntity() { }

        public EmployeeEntity(string firstName, string lastName)
        {
            PartitionKey = "staff";

            RowKey = string.Concat(firstName, " ", lastName);

            FirstName = firstName;

            LastName = lastName;
        }
    }
}