using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

using Informatics.MauiSurdeg.Pages;

namespace Informatics.MauiSurdeg.ViewModels;

public partial class EmployeesViewModel : BaseViewModel
{
    private readonly IEmployeeService _employeeService;
    public ObservableCollection<Employee> Employees { get; } = new();
    public EmployeesViewModel(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
        Title = "Employees";
        RefreshCommand = new AsyncRelayCommand(LoadEmployeesAsync);
        OpenEmployeeDetailsCommand = new RelayCommand<int>(OpenEmployeeDetails);
        OpenAddEmployeeCommand = new RelayCommand(OpenAddEmployee);
    }
    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand<int> OpenEmployeeDetailsCommand { get; }
    public IRelayCommand OpenAddEmployeeCommand { get; }
    private async Task LoadEmployeesAsync()
    {
        if (IsBusy) return; // Prevent multiple calls
        try
        {
            IsBusy = true;
            var employees = await _employeeService.GetEmployeesAsync();
            Employees.Clear();
            foreach (var employee in employees)
            {
                Employees.Add(employee);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading employees: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private void OpenAddEmployee() => OpenEmployeeDetails(-1);
    private async void OpenEmployeeDetails(int id)
    {
        await Shell.Current.GoToAsync($"{nameof(EmployeeDetailsPage)}?employeeId={id}");
    }
}
