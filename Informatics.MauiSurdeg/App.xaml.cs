using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Pages;

namespace Informatics.MauiSurdeg;

public partial class App : Application
{
    private static App _instance;
    public static int UserId { get; private set; }

    public App()
    {
        InitializeComponent();
        MainPage = new LoginShell();
        _instance = this; // Spara referensen till instansen av appen
    }

    public static async Task NavigateToMainShell(int userId)
    {
        if (_instance?.MainPage != null)
        {
            await _instance.MainPage.Dispatcher.DispatchAsync(() =>
            {
                UserId = userId;
                _instance.MainPage = new MainShell();
            });
        }
    }
}
