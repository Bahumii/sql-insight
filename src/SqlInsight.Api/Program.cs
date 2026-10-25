using Microsoft.EntityFrameworkCore;
using SqlInsight.Api.Models;
using SqlInsight.Api.Contracts;
using SqlInsight.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the database context with dependency injection.
// The SQL Server connection string will be read from configuration.
builder.Services.AddDbContext<SqlInsightDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Read the connection string from application configuration.
// If it is missing, stop immediately with a clear error.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found. " +
        "Check the Development environment and User Secrets.");

// Register the database context with SQL Server.
builder.Services.AddDbContext<SqlInsightDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/operations", async (
    SqlInsightDbContext db,
    CancellationToken cancellationToken) =>
{
    // Read-only query: we are retrieving data, not modifying it.
    var operations = await db.OperationLogs
        .AsNoTracking()
        .OrderByDescending(operation => operation.OccurredAtUtc)
        .Take(100)
        .Select(operation => new OperationLogResponse
        {
            Id = operation.Id,
            OperationType = operation.OperationType,
            Description = operation.Description,
            OccurredAtUtc = operation.OccurredAtUtc,
            Status = operation.Status
        })
        .ToListAsync(cancellationToken);

    return Results.Ok(operations);
})
.WithName("GetOperations")
.WithSummary("Get the latest 100 operation logs");

app.MapPost("/api/operations", async (
    CreateOperationLogRequest request,
    SqlInsightDbContext db,
    CancellationToken cancellationToken) =>
{
    // Reject incomplete requests before attempting to save.
    if (string.IsNullOrWhiteSpace(request.OperationType) ||
        string.IsNullOrWhiteSpace(request.Description) ||
        string.IsNullOrWhiteSpace(request.Status))
    {
        return Results.BadRequest(new
        {
            message = "OperationType, Description and Status are required."
        });
    }

    // Keep the accepted status values consistent.
    if (request.Status != "Success" && request.Status != "Failed")
    {
        return Results.BadRequest(new
        {
            message = "Status must be 'Success' or 'Failed'."
        });
    }

    var operation = new OperationLog
    {
        OperationType = request.OperationType.Trim(),
        Description = request.Description.Trim(),
        Status = request.Status,
        OccurredAtUtc = DateTime.UtcNow
    };

    db.OperationLogs.Add(operation);

    await db.SaveChangesAsync(cancellationToken);

    var response = new OperationLogResponse
    {
        Id = operation.Id,
        OperationType = operation.OperationType,
        Description = operation.Description,
        OccurredAtUtc = operation.OccurredAtUtc,
        Status = operation.Status
    };

    return Results.Created(
        $"/api/operations/{operation.Id}",
        response);
})
.WithName("CreateOperation")
.WithSummary("Record a new operation");

app.Run();

