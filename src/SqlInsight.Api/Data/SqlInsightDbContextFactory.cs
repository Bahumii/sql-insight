
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SqlInsight.Api.Data;

/// <summary>
/// Creates the database context when Entity Framework runs design-time
/// commands such as migrations. This avoids relying on API startup
/// to provide the database configuration.
/// </summary>
public class SqlInsightDbContextFactory
    : IDesignTimeDbContextFactory<SqlInsightDbContext>
{
    public SqlInsightDbContext CreateDbContext(string[] args)
    {
        // Load the connection string from User Secrets, then allow
        // an environment variable to override it if one is supplied.
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<SqlInsightDbContextFactory>(
                optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found " +
                "in User Secrets or environment variables.");

        // Configure Entity Framework to connect to SQL Server.
        var options = new DbContextOptionsBuilder<SqlInsightDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new SqlInsightDbContext(options);
    }
}
