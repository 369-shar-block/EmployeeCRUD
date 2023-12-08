using Microsoft.AspNetCore.Mvc;
using EmployeeScreeningTest.Models;
using EmployeeScreeningTest.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeScreeningTest.Controllers
{
    /// <summary>
    /// Controller for managing Employee entities. Provides endpoints for CRUD operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Initializes a new instance of the EmployeesController.
        /// </summary>
        /// <param name="employeeService">The service responsible for employee data operations.</param>
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        /// <summary>
        /// Retrieves the details of a specific employee.
        /// </summary>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>A status code of 200 (Ok) along with the employee's details if found, otherwise a 404 (NotFound).</returns>
        // GET: /Employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }


        /// <summary>
        /// Retrieves a list of all employees.
        /// </summary>
        /// <returns>An enumerable collection of Employee objects. If no employees exist, an empty collection is returned.</returns>
        // GET: /Employees
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeService.GetAllEmployeesAsync();
        }


        /// <summary>
        /// Creates a new employee record. The employee data is expected in the request body.
        /// </summary>
        /// <param name="employee">The employee object to create.</param>
        /// <returns>A status code of 201 (Created) along with the created employee's details. The response includes a URI to the newly created employee resource.</returns>
        // POST: /Employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }


        /// <summary>
        /// Updates an existing employee's information. The employee ID is specified in the URL, and the updated data is provided in the request body.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to update.</param>
        /// <param name="employee">The updated employee object.</param>
        /// <returns>A status code of 204 (No Content) if the update is successful. If the ID in the URL does not match the ID in the body, a 400 (Bad Request) is returned.</returns>
        // PUT: /Employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            await _employeeService.UpdateEmployeeAsync(employee);
            return NoContent();
        }


        /// <summary>
        /// Deletes an employee record identified by the given ID. 
        /// </summary>
        /// <param name="id">The unique identifier of the employee to be deleted.</param>
        /// <returns>A status code of 204 (No Content) if the deletion is successful.</returns>
        // DELETE: /Employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
