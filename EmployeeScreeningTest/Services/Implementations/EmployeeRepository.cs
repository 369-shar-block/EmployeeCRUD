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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Container _container;
        private readonly IDistributedCache _cache;

        public EmployeeRepository(CosmosClient cosmosClient, string databaseName, string containerName, IDistributedCache cache)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
            _cache = cache;
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _container.CreateItemAsync(employee, new PartitionKey(employee.Id));
            // Invalidate the GetAllEmployees cache when a new employee is created
            await _cache.RemoveAsync("all_employees");
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            await _container.DeleteItemAsync<Employee>(id, new PartitionKey(id));
            // Invalidate the cache for this specific employee
            await _cache.RemoveAsync(id);
            // Invalidate the GetAllEmployees cache when an employee is deleted
            await _cache.RemoveAsync("all_employees");
        }

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
