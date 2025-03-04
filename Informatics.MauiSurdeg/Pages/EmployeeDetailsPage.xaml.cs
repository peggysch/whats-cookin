using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages;

[QueryProperty(nameof(EmployeeId), "employeeId")]
public partial class EmployeeDetailsPage : ContentPage
{
	private readonly EmployeeDetailsViewModel _viewModel;
	private int _employeeId;
	public int EmployeeId
	{
		get => _employeeId;
		set
		{
			_employeeId = value;
			LoadEmployeeAsync(_employeeId);
		}
	}
	public EmployeeDetailsPage(EmployeeDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}
	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		if (_employeeId > 0)
		{
			await LoadEmployeeAsync(_employeeId);
		}
	}
	private async Task LoadEmployeeAsync(int employeeId)
	{
		if (_viewModel != null)
		{
			await _viewModel.LoadEmployeeAsync(employeeId);
		}
	}
}