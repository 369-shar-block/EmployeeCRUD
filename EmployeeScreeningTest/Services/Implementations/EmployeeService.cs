using EmployeeScreeningTest.Models;
using EmployeeScreeningTest.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeScreeningTest.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.CreateEmployeeAsync(employee);
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            await _employeeRepository.DeleteEmployeeAsync(id);
        }

        public async Task<Employee> GetEmployeeAsync(string id)
        {
            return await _employeeRepository.GetEmployeeAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
    }
}
