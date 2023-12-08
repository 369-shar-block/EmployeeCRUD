using EmployeeScreeningTest.Models;
using EmployeeScreeningTest.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json; // Add this to handle JSON serialization
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeScreeningTest.Services.Implementations
{
    /// <summary>
    /// Repository implementation for Employee data management, interfacing with Azure Cosmos DB and Redis Cache.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Container _container;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Initializes a new instance of EmployeeRepository with specified Cosmos DB container and cache.
        /// </summary>
        /// <param name="cosmosClient">The CosmosClient instance.</param>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="cache">The distributed cache instance.</param>
        public EmployeeRepository(CosmosClient cosmosClient, string databaseName, string containerName, IDistributedCache cache)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            _cache = cache;
        }

        // <summary>
        /// Creates a new employee record in the database.
        /// Invalidates the "all employees" cache entry after creation to ensure cache consistency.
        /// </summary>
        /// <param name="employee">The employee to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _container.CreateItemAsync(employee, new PartitionKey(employee.Id));
            // Invalidate the GetAllEmployees cache when a new employee is created
            await _cache.RemoveAsync("all_employees");
        }


        /// <summary>
        /// Asynchronously deletes an employee record from the database by their ID.
        /// It also invalidates the cache for the specific employee and the cache for all employees,
        /// ensuring the cache remains consistent with the database state.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to be deleted.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteEmployeeAsync(string id)
        {
            await _container.DeleteItemAsync<Employee>(id, new PartitionKey(id));
            // Invalidate the cache for this specific employee
            await _cache.RemoveAsync(id);
            // Invalidate the GetAllEmployees cache when an employee is deleted
            await _cache.RemoveAsync("all_employees");
        }


        /// <summary>
        /// Asynchronously retrieves an employee's details by their ID. It first checks the cache for the data.
        /// If not found in the cache, it fetches the data from Cosmos DB and then caches it for future requests.
        /// If the employee is not found in the database, null is returned.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to retrieve.</param>
        /// <returns>The employee object if found; otherwise, null.</returns>
        public async Task<Employee> GetEmployeeAsync(string id)
        {
            Employee employee = null;
            string cachedEmployee = await _cache.GetStringAsync(id);

            if (!string.IsNullOrEmpty(cachedEmployee))
            {
                employee = JsonConvert.DeserializeObject<Employee>(cachedEmployee);
            }
            else
            {
                try
                {
                    ItemResponse<Employee> response = await _container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                    employee = response.Resource;
                    if (employee != null)
                    {
                        await _cache.SetStringAsync(id, JsonConvert.SerializeObject(employee), new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                        });
                    }
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Employee not found
                    employee = null;
                }
            }

            return employee;
        }


        /// <summary>
        /// Asynchronously retrieves all employee records. It first checks the cache for the data.
        /// If not found in the cache, it fetches the data from Cosmos DB, caches it for future requests, and then returns the result.
        /// The cache is set with a 30-minute expiration for data freshness.
        /// </summary>
        /// <returns>An enumerable collection of all employees.</returns>
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            string cachedEmployees = await _cache.GetStringAsync("all_employees");
            IEnumerable<Employee> employees;

            if (!string.IsNullOrEmpty(cachedEmployees))
            {
                employees = JsonConvert.DeserializeObject<List<Employee>>(cachedEmployees);
            }
            else
            {
                var query = _container.GetItemQueryIterator<Employee>(new QueryDefinition("SELECT * FROM c"));
                List<Employee> results = new List<Employee>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    results.AddRange(response.ToList());
                }
                employees = results;
                if (employees.Any())
                {
                    await _cache.SetStringAsync("all_employees", JsonConvert.SerializeObject(employees), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
                    });
                }
            }

            return employees;
        }


        /// <summary>
        /// Asynchronously updates an employee's record in the database. After updating, the employee's
        /// new details are also updated in the cache with a 30-minute expiration. Additionally, this method
        /// invalidates the cached list of all employees to ensure consistency across individual and collective queries.
        /// </summary>
        /// <param name="employee">The employee object with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _container.UpsertItemAsync(employee, new PartitionKey(employee.Id));
            // Update the cache with the new employee details
            await _cache.SetStringAsync(employee.Id, JsonConvert.SerializeObject(employee), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
            });
            // Invalidate the GetAllEmployees cache when an employee is updated
            await _cache.RemoveAsync("all_employees");
        }
    }
}
