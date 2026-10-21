namespace SqlInsight.Api.Models;

/// <summary>
/// Represents one operation recorded by the SQL Insight application.
/// Each instance will eventually correspond to a row in the database.
/// </summary>
public class OperationLog
{
    /// <summary>
    /// Unique identifier for this operation log entry.
    /// Entity Framework Core can use this property as the primary key.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Describes the type of operation, such as INSERT, UPDATE or DELETE.
    /// </summary>
    public string OperationType { get; set; } = string.Empty;

    /// <summary>
    /// Provides a human-readable explanation of what happened.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Records when the operation occurred, using UTC time.
    /// </summary>
    public DateTime OccurredAtUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates the outcome, for example Success or Failed.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
