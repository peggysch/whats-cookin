namespace Informatics.MauiSurdeg.Pages;
using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Converters;


public partial class AddRecipePage : ContentPage
{
	public AddRecipePage(AddRecipeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;

		Resources.Add("IngredientNameConverter", new IngredientNameConverter(viewModel.AvailableIngredients));
	}
}