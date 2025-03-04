using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages;

[QueryProperty(nameof(DepartmentId), "departmentId")]
public partial class DepartmentDetailsPage : ContentPage
{
	private readonly DepartmentDetailsViewModel _viewModel;
	private int _departmentId;
	public int DepartmentId
	{
		get => _departmentId;
		set
		{
			_departmentId = value;
			LoadDepartmentAsync(_departmentId);
		}
	}
	public DepartmentDetailsPage(DepartmentDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}
	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		if (_departmentId > 0)
		{
			await LoadDepartmentAsync(_departmentId);
		}
	}
	private async Task LoadDepartmentAsync(int departmentId)
	{
		if (_viewModel != null)
		{
			await _viewModel.LoadDepartmentAsync(departmentId);
		}
	}
}