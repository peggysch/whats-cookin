using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as LoginViewModel;
	}
}

