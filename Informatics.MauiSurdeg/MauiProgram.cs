using Informatics.MauiSurdeg.Contexts;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;
using Informatics.MauiSurdeg.DataBase;

using Informatics.MauiSurdeg.Interfaces;
using Informatics.MauiSurdeg.Services;
using Informatics.MauiSurdeg.ViewModels;
using Informatics.MauiSurdeg.Pages;

using OpenAI.Chat;
using OpenAI;

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
				fonts.AddFont("LibreBaskerville-Bold.ttf", "LibreBaskervilleBold");
				fonts.AddFont("LibreBaskerville-Regular.ttf", "LibreBaskervilleRegular");
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
		// If a SQL Server connection string is configured in appsettings.json, use it (original setup).
		// Otherwise fall back to a local SQLite file so the app can run without a database server.
		var connectionString = builder.Configuration["AppSettings:WhatsCookinDatabase"];
		var useSqlServer = !string.IsNullOrWhiteSpace(connectionString);
		var sqlitePath = Path.Combine(FileSystem.AppDataDirectory, "whatscookin.db");

		builder.Services.AddDbContext<WhatsCookinContext>(options =>
		{
			if (useSqlServer)
				options.UseSqlServer(connectionString);
			else
				options.UseSqlite($"Data Source={sqlitePath}");
		});


		builder.Services.AddScoped<IRecipeService, RecipeService>();
		builder.Services.AddScoped<IAppUserService, AppUserService>();
		builder.Services.AddScoped<IRatingService, RatingService>();
		builder.Services.AddScoped<ICommentService, CommentService>();
		builder.Services.AddScoped<IIngredientService, IngredientService>();
		builder.Services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();
		
		builder.Services.AddHttpClient<IRecipeSuggestionService, RecipeSuggestionService>(client =>
		{
			client.BaseAddress = new Uri("https://api.openai.com/");
			var apiKey = builder.Configuration["AppSettings:OpenAIApiKey"];
			if (!string.IsNullOrWhiteSpace(apiKey))
				client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
		});

		// Register ViewModels
		builder.Services.AddSingleton<LoginViewModel>();
		builder.Services.AddSingleton<ProfileViewModel>();
		builder.Services.AddSingleton<SettingsViewModel>();
		builder.Services.AddSingleton<FeedViewModel>();
		builder.Services.AddSingleton<AddRecipeViewModel>();
		builder.Services.AddSingleton<RecipeSuggestionsViewModel>();


		// Register Pages
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<SettingsPage>();
		builder.Services.AddTransient<FeedPage>();
		builder.Services.AddTransient<AddRecipePage>();
		builder.Services.AddTransient<RecipeSuggestionsPage>();



#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

		// Create and fill the local database on first start
		if (!useSqlServer)
		{
			using var scope = app.Services.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<WhatsCookinContext>();
			DatabaseSeeder.EnsureCreatedAndSeeded(db);
		}

		return app;
	}
}
