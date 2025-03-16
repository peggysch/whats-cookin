using CommunityToolkit.Mvvm.Input;
using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages;

public partial class FeedPage : ContentPage
{
	private readonly FeedViewModel _viewModel;

	public FeedPage(FeedViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await (_viewModel.LoadRecipesCommand as AsyncRelayCommand)?.ExecuteAsync(null);

	}
}