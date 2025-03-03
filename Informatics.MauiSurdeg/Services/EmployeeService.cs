using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Microsoft.EntityFrameworkCore;

namespace Informatics.MauiSurdeg.Services;

public class EmployeeService : IEmployeeService
{
    private readonly UniversityContext _context;
    public EmployeeService(UniversityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _context.Employees
        .AsNoTracking()
        .Include(e => e.Department)
        .ToListAsync();
    }
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
        .AsNoTracking()
        .Include(e => e.Department)
        .FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task<Employee?> GetEmployeeByNoAsync(string empNo)
    {
        return await _context.Employees
        .AsNoTracking()
        .Include(e => e.Department)
        .FirstOrDefaultAsync(e => e.Number == empNo);
    }
    public async Task<Employee> SaveEmployeeAsync(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));
        try
        {
            if (employee.Department != null)
                employee.DepartmentId = employee.Department.Id;
            employee.Department = null; // Prevent EF from trying to insert Department
            if (employee.Id > 0) // Existing Employee
            {
                _context.Employees.Update(employee);
            }
            else // New Employee
            {
                await _context.Employees.AddAsync(employee);
            }
            await _context.SaveChangesAsync();
            return employee;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error saving employee.", ex);
        }
    }
    public async Task<bool> DeleteEmployeeByIdAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
            return false;
        try
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting employee.", ex);
        }
    }
    public async Task<bool> DeleteEmployeeByNoAsync(string empNo)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Number == empNo);
        if (employee == null)
            return false;
        try
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting employee.", ex);
        }
    }
}
