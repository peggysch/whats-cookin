using Informatics.MauiSurdeg.Contexts;
using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;

using Microsoft.EntityFrameworkCore;

namespace Informatics.MauiSurdeg.Services;
public class DepartmentService : IDepartmentService
{
    private readonly UniversityContext _context;
    public DepartmentService(UniversityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<IEnumerable<Department>> GetDepartmentsAsync()
    {
        return await _context.Departments
        .AsNoTracking()
        .Include(d => d.Employees) // Load employees
        .ToListAsync();
    }
    public async Task<Department?> GetDepartmentByIdAsync(int id)
    {
        return await _context.Departments
        .AsNoTracking()
        .Include(d => d.Employees) // Load employees
        .FirstOrDefaultAsync(d => d.Id == id);
    }
    public async Task<Department?> GetDepartmentByNameAsync(string name)
    {
        return await _context.Departments
        .AsNoTracking()
        .Include(d => d.Employees)
        .FirstOrDefaultAsync(d => d.Name == name);
    }
    public async Task<Department> SaveDepartmentAsync(Department department)
    {
        if (department == null)
            throw new ArgumentNullException(nameof(department));
        try
        {
            if (department.Id > 0)
            {
                // Update existing department
                _context.Departments.Update(department);
            }
            else
            {
                // Ensure EF does not try to insert an explicit ID
                department.Id = 0;
                await _context.Departments.AddAsync(department);
            }
            await _context.SaveChangesAsync();
            return department;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error saving department.", ex);
        }
    }
    public async Task<bool> DeleteDepartmentByIdAsync(int id)
    {
        var department = await _context.Departments.FindAsync(id);
        if (department == null)
            return false;
        try
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting department.", ex);
        }
    }
    public async Task<bool> DeleteDepartmentByNameAsync(string name)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == name);
        if (department == null)
            return false;
        try
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("Error deleting department.", ex);
        }
    }
}
