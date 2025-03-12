using Informatics.MauiSurdeg.Contexts;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Services;
using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Pages;

namespace Informatics.MauiSurdeg;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		var assembly = Assembly.GetExecutingAssembly();
		string appsettingsFileName = "Informatics.MauiSurdeg.appsettings.json";
		using (var stream = assembly.GetManifestResourceStream(appsettingsFileName))
		{
			if (stream != null)
			{
				builder.Configuration.AddJsonStream(stream);
			}
		}

		// Add database context to the service collection
		builder.Services.AddDbContext<UniversityContext>(options =>
		{
			var connectionString =
			 builder.Configuration.GetConnectionString("UniversityDatabase");
			options.UseSqlServer(connectionString);
		});

		var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");
		Console.WriteLine($"Connection string loaded: {connectionString != null}");
		// Only print first few chars to avoid showing credentials in logs
		if (connectionString != null)
			Console.WriteLine($"Connection starts with: {connectionString.Substring(0, Math.Min(15, connectionString.Length))}...");

		builder.Services.AddScoped<IRecipeService, RecipeService>();
		builder.Services.AddScoped<IAppUserService, AppUserService>();
		builder.Services.AddScoped<IRatingService, RatingService>();
		builder.Services.AddScoped<ICommentService, CommentService>();
		builder.Services.AddScoped<IIngredientService, IngredientService>();
		builder.Services.AddScoped<IRatingService, RatingService>();

		// Register ViewModels
		builder.Services.AddTransient<LoginViewModel>();

		// Register Pages
		builder.Services.AddTransient<LoginPage>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
