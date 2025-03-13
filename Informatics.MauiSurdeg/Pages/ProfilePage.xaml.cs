using Informatics.MauiSurdeg.ViewModels;

namespace Informatics.MauiSurdeg.Pages
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as ProfileViewModel;
            //viewModel?.LoadUserDataCommand.Execute(null);
        }
    }
}
