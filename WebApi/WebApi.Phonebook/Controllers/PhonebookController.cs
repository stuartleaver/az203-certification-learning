
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Phonebook.Models;

namespace WebApi.Phonebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhonebookController : ControllerBase
    {
        private readonly PhonebookContext _context;
        
        public PhonebookController(PhonebookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _context.PhonebookEntries.ToList();
        }

        [HttpGet("{id}", Name = "GetPerson")]
        public IActionResult GetById(long id)
        {
            var item = _context.PhonebookEntries.FirstOrDefault(t => t.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest();
            }

            _context.PhonebookEntries.Add(person);

            _context.SaveChanges();

            return CreatedAtRoute("GetPerson", new { id = person.Id }, person);
        }
    }
}