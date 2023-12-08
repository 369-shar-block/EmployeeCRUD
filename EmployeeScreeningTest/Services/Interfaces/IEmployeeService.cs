using EmployeeScreeningTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeScreeningTest.Services.Interfaces
{
    /// <summary>
    /// Provides an abstraction layer for employee-related operations, decoupling the 
    /// controllers from direct data layer manipulation.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Asynchronously retrieves an employee's details by their ID.
        /// </summary>
        /// <param name="id">The unique identifier for the employee.</param>
        /// <returns>The employee object if found; otherwise, null.</returns>
        Task<Employee> GetEmployeeAsync(string id);

        /// <summary>
        /// Asynchronously retrieves a list of all employees.
        /// </summary>
        /// <returns>A collection of all employee objects.</returns>
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();

        /// <summary>
        /// Asynchronously creates a new employee record.
        /// </summary>
        /// <param name="employee">The employee object to create.</param>
        /// <returns>A task representing the asynchronous create operation.</returns>
        Task CreateEmployeeAsync(Employee employee);

        /// <summary>
        /// Asynchronously updates an existing employee's record.
        /// </summary>
        /// <param name="employee">The employee object with updated information.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        Task UpdateEmployeeAsync(Employee employee);

        /// <summary>
        /// Asynchronously deletes an employee's record from the system by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to delete.</param>
        /// <returns>A task representing the asynchronous delete operation.</returns>
        Task DeleteEmployeeAsync(string id);
    }
}
