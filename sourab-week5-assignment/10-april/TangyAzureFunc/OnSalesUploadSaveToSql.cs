using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TangyAzureFunc.Data;
using TangyAzureFunc.Models;

namespace TangyAzureFunc;

public class OnSalesUploadSaveToSql
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<OnSalesUploadSaveToSql> _logger;

    public OnSalesUploadSaveToSql(AppDbContext dbContext, ILogger<OnSalesUploadSaveToSql> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Function("OnSalesUploadSaveToSql")]
    public async Task Run([QueueTrigger("salesrequests", Connection = "AzureWebJobsStorage")] string queueMessage)
    {
        try
        {
            var json = DecodeIfBase64(queueMessage);
            var request = JsonSerializer.Deserialize<SalesRequest>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (request is null)
            {
                _logger.LogWarning("Queue message could not be deserialized to SalesRequest.");
                return;
            }

            if (string.IsNullOrWhiteSpace(request.Id))
            {
                request.Id = Guid.NewGuid().ToString();
            }

            _dbContext.SalesRequests.Add(request);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Sales request {RequestId} saved to SQL Server.", request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to persist queue message to SQL Server.");
            throw;
        }
    }

    private static string DecodeIfBase64(string value)
    {
        try
        {
            var bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return value;
        }
    }
}
