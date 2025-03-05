using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages;
public partial class EmployeesPage : ContentPage
{
	public EmployeesPage(EmployeesViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as EmployeesViewModel;
		viewModel?.RefreshCommand.Execute(null);
	}
}