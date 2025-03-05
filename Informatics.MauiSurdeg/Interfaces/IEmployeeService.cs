using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Interfaces;
public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee?> GetEmployeeByNoAsync(string empNo);
    Task<Employee> SaveEmployeeAsync(Employee employee);
    Task<bool> DeleteEmployeeByIdAsync(int id);
    Task<bool> DeleteEmployeeByNoAsync(string empNo);
}

