using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PostSimulator
{

    class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Id { get; set; }
    }
    class Program
    {
        static async Task PostCustomersAsync(string baseUrl, string jsonContent)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //Set the base address of the REST server
                    client.BaseAddress = new Uri(baseUrl);

                    //Define content type
                    StringContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    //Send POST request to AddCustomers endpoint
                    HttpResponseMessage response = await client.PostAsync("/customers", content);

                    //Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Customers added successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add customers. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static List<Customer> GenerateSimulatedCustomers(int startId)
        {
            Random random = new Random();
            List<Customer> customers = new List<Customer>();

            //Sample first names and last names
            string[] firstNames = { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
            string[] lastNames = { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

            //Generate at least 2 customers
            int numCustomers = random.Next(2, 5);

            //Generate customers with random age and increasing ID
            for (int i = 0; i < numCustomers; i++)
            {
                Customer customer = new Customer
                {
                    FirstName = firstNames[random.Next(firstNames.Length)],
                    LastName = lastNames[random.Next(lastNames.Length)],
                    Age = random.Next(10, 91), //Age between 10 and 90
                    Id = startId + i + 1 //Increasing ID starting from startId + 1
                };
                customers.Add(customer);
            }

            return customers;
        }

        static async Task<List<Customer>> GetExistingCustomersAsync(string baseUrl)
        {
            List<Customer> existingCustomers = new List<Customer>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    //Set the base address of the REST server
                    client.BaseAddress = new Uri(baseUrl);

                    //Send GET request to retrieve existing customers
                    HttpResponseMessage response = await client.GetAsync("/customers");

                    //Check if request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        existingCustomers = JsonConvert.DeserializeObject<List<Customer>>(jsonResponse);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to get existing customers. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return existingCustomers;
        }

        static async Task Main(string[] args)
        {
            //Base URL of the REST server
            string baseUrl = "http://localhost:5050";

            //Get existing customers from the server
            List<Customer> existingCustomers = await GetExistingCustomersAsync(baseUrl);

            //Find the highest existing customer ID
            int maxId = existingCustomers.Max(c => c.Id);

            //Generate simulated customers
            List<Customer> simulatedCustomers = GenerateSimulatedCustomers(maxId);

            //Serialize customers to JSON
            string jsonContent = JsonConvert.SerializeObject(simulatedCustomers);

            //Call the AddCustomers endpoint with simulated data
            await PostCustomersAsync(baseUrl, jsonContent);
        }


    }


}