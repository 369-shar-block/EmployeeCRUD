using EmployeeScreeningTest.Models;
using EmployeeScreeningTest.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeScreeningTest.Services.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Container _container;

        public EmployeeRepository(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _container.CreateItemAsync(employee, new PartitionKey(employee.Id));
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            await _container.DeleteItemAsync<Employee>(id, new PartitionKey(id));
        }

        public async Task<Employee> GetEmployeeAsync(string id)
        {
            try
            {
                ItemResponse<Employee> response = await _container.ReadItemAsync<Employee>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                
                return null;
            }
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var query = _container.GetItemQueryIterator<Employee>(new QueryDefinition("SELECT * FROM c"));
            List<Employee> results = new List<Employee>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _container.UpsertItemAsync(employee, new PartitionKey(employee.Id));
        }
    }
}
