using Microsoft.EntityFrameworkCore;

namespace WebApi.Phonebook.Models
{
    public class PhonebookContext : DbContext
    {
        public PhonebookContext(DbContextOptions<PhonebookContext> options) : base(options)
        {

        }

        public DbSet<Person> PhonebookEntries {get; set; }
    }
}