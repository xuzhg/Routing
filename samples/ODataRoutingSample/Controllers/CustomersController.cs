using Microsoft.AspNetCore.Mvc;
using ODataRoutingSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataRoutingSample.Controllers
{
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Customer
            {
                Id = index,
                Name = "Name + " + index
            })
            .ToArray();
        }
    }
}
