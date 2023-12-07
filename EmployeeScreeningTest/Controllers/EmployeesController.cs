using Microsoft.AspNetCore.Mvc;
using EmployeeScreeningTest.Models;
using EmployeeScreeningTest.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EmployeeScreeningTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

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

        // GET: /Employees
        [HttpGet]
        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _employeeService.GetAllEmployeesAsync();
        }

        // POST: /Employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }

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

        // DELETE: /Employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}
