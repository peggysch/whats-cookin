using Informatics.MauiSurdeg.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace Informatics.MauiSurdeg.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    
}
