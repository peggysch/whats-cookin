using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class DepartmentDetailsViewModel : BaseViewModel
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;
    public ObservableCollection<Employee> Employees { get; } = new();
    [ObservableProperty]
    private Department department;
    public DepartmentDetailsViewModel(IDepartmentService departmentService, IEmployeeService
    employeeService)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
        SaveDepartmentCommand = new AsyncRelayCommand(SaveDepartmentAsync);
        DeleteDepartmentCommand = new AsyncRelayCommand(DeleteDepartmentAsync);
    }
    public IAsyncRelayCommand SaveDepartmentCommand { get; }
    public IAsyncRelayCommand DeleteDepartmentCommand { get; }
    public async Task LoadDepartmentAsync(int departmentId)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            if (departmentId == -1)
            {
                // If departmentId is -1, create a new department
                Department = new Department();
            }
            else
            {
                // Otherwise, load existing department
                Department = await _departmentService.GetDepartmentByIdAsync(departmentId) ?? new
                Department();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading department: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task SaveDepartmentAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            await _departmentService.SaveDepartmentAsync(Department);
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving department: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private async Task DeleteDepartmentAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            await _departmentService.DeleteDepartmentByIdAsync(Department.Id);
            await Shell.Current.GoToAsync(".."); // Navigate back
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting department: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
