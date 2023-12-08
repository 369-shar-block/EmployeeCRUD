using EmployeeScreeningTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeScreeningTest.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for the repository handling the data operations for Employee entities.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Retrieves an employee by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the employee to retrieve.</param>
        /// <returns>The employee if found; otherwise, null.</returns>
        Task<Employee> GetEmployeeAsync(string id);

        /// <summary>
        /// Retrieves all employees in the repository.
        /// </summary>
        /// <returns>An enumerable collection of Employee objects.</returns>
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        /// <summary>
        /// Adds a new employee to the repository.
        /// </summary>
        /// <param name="employee">The Employee object to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateEmployeeAsync(Employee employee);

        /// <summary>
        /// Updates an existing employee's information in the repository.
        /// </summary>
        /// <param name="employee">The updated Employee object.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateEmployeeAsync(Employee employee);

        /// <summary>
        /// Deletes an employee from the repository by their unique identifier.
        /// </summary>
        /// <param name="id">The ID of the employee to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteEmployeeAsync(string id);
    }
}
