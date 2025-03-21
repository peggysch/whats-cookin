using Informatics.MauiSurdeg.Contexts;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
		builder.Services.AddDbContext<WhatsCookinContext>(options =>
		{
			var connectionString = builder.Configuration["AppSettings:WhatsCookinDatabase"]; ;
			options.UseSqlServer(connectionString);
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
			client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["AppSettings:OpenAIApiKey"]}");
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

		return builder.Build();
	}
}
