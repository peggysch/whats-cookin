using Microsoft.Maui.Controls;

namespace Informatics.MauiSurdeg
{
    public partial class AppShell : Shell
    {
        private static AppShell _current;
        public static AppShell Current => _current ??= new AppShell();

        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get => _isUserLoggedIn;
            set
            {
                if (_isUserLoggedIn != value)
                {
                    _isUserLoggedIn = value;
                    OnPropertyChanged(nameof(IsUserLoggedIn));

                    // Enable Flyout when logged in
                    FlyoutBehavior = value ? FlyoutBehavior.Locked : FlyoutBehavior.Disabled;
                }
            }
        }

        public AppShell()
        {
            InitializeComponent();
            _current = this;
            BindingContext = this;
        }
    }
}
