namespace SqlInsight.Api.Contracts;

/// <summary>
/// Contains the fields a client must send to record an operation.
/// The API will generate the ID and timestamp.
/// </summary>
public class CreateOperationLogRequest
{
    public string OperationType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}
