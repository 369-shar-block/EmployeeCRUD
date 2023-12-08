using EmployeeScreeningTest.Models;
using EmployeeScreeningTest.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeScreeningTest.Services.Implementations
{
    /// <summary>
    /// Implements the business logic for managing employees, delegating data operations to the EmployeeRepository.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        /// <summary>
        /// Initializes a new instance of the EmployeeService with the specified repository.
        /// </summary>
        /// <param name="employeeRepository">The repository handling employee data operations.</param>
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <inheritdoc/>
        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.CreateEmployeeAsync(employee);
        }

        /// <inheritdoc/>
        public async Task DeleteEmployeeAsync(string id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
        }

        /// <inheritdoc/>
        public async Task<Employee> GetEmployeeAsync(string id)
        {
            return await _employeeRepository.GetEmployeeAsync(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
    }
}
