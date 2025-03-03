using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class EmployeeDetailsViewModel : BaseViewModel
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    public ObservableCollection<Department> Departments { get; } = new();
    [ObservableProperty]
    private Employee employee;
    public EmployeeDetailsViewModel(IEmployeeService employeeService,
    IDepartmentService departmentService)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        SaveEmployeeCommand = new AsyncRelayCommand(SaveEmployeeAsync);
        DeleteEmployeeCommand = new AsyncRelayCommand(DeleteEmployeeAsync);
    }
    public IAsyncRelayCommand SaveEmployeeCommand { get; }
    public IAsyncRelayCommand DeleteEmployeeCommand { get; }
    public async Task LoadEmployeeAsync(int employeeId)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            if (employeeId == -1)
            {
                // If employeeId is -1, create a new employee
                Employee = new Employee();
            }
            else
            {
                // Otherwise, load existing employee
                Employee = await _employeeService.GetEmployeeByIdAsync(employeeId)
                ?? new Employee();
            }
            await LoadDepartmentsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading employee: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task LoadDepartmentsAsync()
    {
        try
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            Departments.Clear();
            foreach (var dept in departments)
                Departments.Add(dept);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading departments: {ex.Message}");
        }
    }
    private async Task SaveEmployeeAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            await _employeeService.SaveEmployeeAsync(Employee);
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving employee: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task DeleteEmployeeAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            await _employeeService.DeleteEmployeeByIdAsync(Employee.Id);
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting employee: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}