using Informatics.MauiSurdeg.Pages;

namespace Informatics.MauiSurdeg;
public partial class AppShell : Shell
{
public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
		//Routing.RegisterRoute(nameof(FeedPage), typeof(FeedPage))
	}
}
