using Informatics.MauiSurdeg.Models;

namespace Informatics.MauiSurdeg.Interfaces;
public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetDepartmentsAsync();
    Task<Department?> GetDepartmentByIdAsync(int id); // Fetch by DepartmentID
    Task<Department?> GetDepartmentByNameAsync(string name); // Fetch by DeptName
    Task<Department> SaveDepartmentAsync(Department department);
    Task<bool> DeleteDepartmentByIdAsync(int id);
    Task<bool> DeleteDepartmentByNameAsync(string name);
}