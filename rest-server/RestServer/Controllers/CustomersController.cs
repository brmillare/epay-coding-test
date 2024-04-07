using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using System.Linq;

namespace RestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public class Customer
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public int Id { get; set; }
        }

        private static List<Customer> customers = new List<Customer>();

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers(); //add controller services
            }

            //configure HTTP request pipeline
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }

        static CustomersController()
        {
            //check if customers.json file exists, this would contain the initial array of customers to be loaded in memory
            if (System.IO.File.Exists("customers.json"))
            {
                string json = System.IO.File.ReadAllText("customers.json");
                customers = JsonConvert.DeserializeObject<List<Customer>>(json);
            }
        }

        [HttpPost] //POST method
        [Route("/customers")] //route to AddCustomers endpoint
        public IActionResult AddCustomers([FromBody] List<Customer> newCustomers)
        {
            //check if at least two customers will be inserted
            if (newCustomers == null || newCustomers.Count < 2)
            {
                return BadRequest("At least two customers are required.");
            }

            var errors = new List<string>();

            //iterate over each new customer
            foreach (var customer in newCustomers)
            {
                //checks on customer data if valid
                if (string.IsNullOrEmpty(customer.FirstName) ||
                    string.IsNullOrEmpty(customer.LastName) ||
                    customer.Age <= 0 ||
                    customer.Id <= 0)
                {
                    errors.Add("All fields must be supplied.");
                }
                if (customer.Age < 10 || customer.Age > 90)
                {
                    errors.Add("Age must be between 10 and 90.");
                }
                if (customers.Any(c => c.Id == customer.Id))
                {
                    errors.Add("ID already exists.");
                }
            }

            if (errors.Count > 0)
            {
                Log.Warning("Failed to add customers. Errors: {Errors}", errors);
                return BadRequest(errors);
            }

            //iterate over each new customer
            foreach (var newCustomer in newCustomers)
            {
                int index = customers.Count;
                for (int i = 0; i < customers.Count; i++)
                {
                    //check for each existing customer where to place the new customer according to LastName and then FirstName
                    //check if new customer should be inserted before current customer (this is for sorting)
                    if (string.Compare(newCustomer.LastName, customers[i].LastName) < 0 ||
                        (string.Compare(newCustomer.LastName, customers[i].LastName) == 0 &&
                         string.Compare(newCustomer.FirstName, customers[i].FirstName) < 0))
                    {
                        index = i;
                        break;
                    }
                }
                customers.Insert(index, newCustomer); //insert the new customer in the index found
            }

            string json = JsonConvert.SerializeObject(customers);
            System.IO.File.WriteAllText("customers.json", json); //write to the JSON file

            Log.Information("Added {Count} customers", newCustomers.Count);

            return Ok(); //return OK response
        }

        [HttpGet]  //GET method
        [Route("/customers")] //route to GET customers endpoint
        public IActionResult GetCustomers()
        {
            Log.Information("Retrieved {Count} customers", customers.Count);
            return Ok(customers); //return customers
        }
    }
}
