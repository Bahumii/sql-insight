using Microsoft.EntityFrameworkCore;
using SqlInsight.Api.Models;

namespace SqlInsight.Api.Data;

/// <summary>
/// Represents the application's session with the SQL Insight database.
/// Entity Framework Core uses this class to query and save data.
/// </summary>
public class SqlInsightDbContext : DbContext
{
    /// <summary>
    /// Receives the database configuration from the application's
    /// dependency injection container and passes it to EF Core.
    /// </summary>
    public SqlInsightDbContext(
        DbContextOptions<SqlInsightDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Represents the OperationLogs table.
    /// EF Core uses this property to query and save operation log records.
    /// </summary>
    public DbSet<OperationLog> OperationLogs => Set<OperationLog>();
}
