using EmployeeScreeningTest.Models;

namespace EmployeeScreeningTest.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeAsync(string id);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(string id);
    }
}
