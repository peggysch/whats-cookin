using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages;
public partial class DepartmentsPage : ContentPage
{
	public DepartmentsPage(DepartmentsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		var viewModel = BindingContext as DepartmentsViewModel;
		viewModel?.RefreshCommand.Execute(null);
	}
}