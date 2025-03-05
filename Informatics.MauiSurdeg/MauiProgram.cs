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

		builder.Services.AddScoped<IDepartmentService, DepartmentService>();
		builder.Services.AddScoped<IEmployeeService, EmployeeService>();

		// Use Singleton for ViewModels that manage lists
		builder.Services.AddSingleton<EmployeesViewModel>();
		builder.Services.AddSingleton<DepartmentsViewModel>();

		// Use Transient for ViewModels that manage single detail pages
		builder.Services.AddTransient<EmployeeDetailsViewModel>();
		builder.Services.AddTransient<DepartmentDetailsViewModel>();

		// Register Pages
		builder.Services.AddSingleton<EmployeesPage>();
		builder.Services.AddTransient<EmployeeDetailsPage>();
		builder.Services.AddSingleton<DepartmentsPage>();
		builder.Services.AddTransient<DepartmentDetailsPage>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
