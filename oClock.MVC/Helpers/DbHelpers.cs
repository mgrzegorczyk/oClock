using Microsoft.EntityFrameworkCore;
using oClock.MVC.Infrastructure;

namespace oClock.MVC.Helpers;

public static class DbHelpers
{
    public static IServiceCollection AddJiraDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("JiraDatabase");

        // Generate the full path for the database file
        var dbPath = Path.Combine(AppContext.BaseDirectory, "Data", "JIRAANALYZER.FDB");

        // Modify the connection string to use the full path
        connectionString = connectionString.Replace("pathToFile.FDB", dbPath);

        services.AddDbContext<JiraDbContext>(options =>
            options.UseFirebird(connectionString));

        return services;
    }
}