namespace SqlInsight.Api.Contracts;

/// <summary>
/// Defines the operation-log data returned to API clients.
/// </summary>
public class OperationLogResponse
{
    public int Id { get; set; }

    public string OperationType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime OccurredAtUtc { get; set; }

    public string Status { get; set; } = string.Empty;
}
