using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Models;
using Informatics.MauiSurdeg.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Informatics.MauiSurdeg.ViewModels;
public partial class DepartmentsViewModel : BaseViewModel
{
    private readonly IDepartmentService _departmentService;
    public ObservableCollection<Department> Departments { get; } = new();
    public DepartmentsViewModel(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
        Title = "Departments";
        RefreshCommand = new AsyncRelayCommand(LoadDepartmentsAsync);
        OpenDepartmentDetailsCommand = new RelayCommand<int>(OpenDepartmentDetails);
        OpenAddDepartmentCommand = new RelayCommand(OpenAddDepartment);
    }
    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand<int> OpenDepartmentDetailsCommand { get; }
    public IRelayCommand OpenAddDepartmentCommand { get; }
    private async Task LoadDepartmentsAsync()
    {
        if (IsBusy) return; // Prevent multiple calls
        try
        {
            IsBusy = true;
            var departments = await _departmentService.GetDepartmentsAsync();
            Departments.Clear();
            foreach (var department in departments)
            {
                Departments.Add(department);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading departments: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
    private void OpenAddDepartment() => OpenDepartmentDetails(-1);
    private async void OpenDepartmentDetails(int id)
    {
        await Shell.Current.GoToAsync($"{nameof(DepartmentDetailsPage)}?departmentId={id}");
    }
}
