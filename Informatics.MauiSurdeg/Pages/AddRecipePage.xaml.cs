using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Converters;

namespace Informatics.MauiSurdeg.Pages;

public partial class AddRecipePage : ContentPage
{
	public AddRecipePage(AddRecipeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
