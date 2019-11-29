using System.Collections.Generic;
using KeyVault.Secrets.DAL;
using KeyVault.Secrets.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KeyVault.Secrets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;

        private readonly CustomerRepository _repository;

        public CustomersController(ILogger<CustomersController> logger, CustomerRepository repository)
        {
            _logger = logger;

            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _repository.GetCustomers().ToArray();
        }
    }
}
