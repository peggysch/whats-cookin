namespace Informatics.MauiSurdeg.Pages;
using Informatics.MauiSurdeg.ViewModels;

public partial class AddRecipePage : ContentPage
{
	public AddRecipePage(AddRecipeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}