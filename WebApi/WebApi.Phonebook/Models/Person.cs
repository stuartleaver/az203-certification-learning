using System;

namespace WebApi.Phonebook.Models
{
    public class Person
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }
}