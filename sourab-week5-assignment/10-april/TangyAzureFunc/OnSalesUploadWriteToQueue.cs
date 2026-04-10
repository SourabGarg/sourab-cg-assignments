using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Azure.Storage.Queues;
using System.Text;

namespace TangyAzureFunc;

public class OnSalesUploadWriteToQueue
{
    [Function("OnSalesUploadWriteToQueue")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        string body;
        using (var reader = new StreamReader(req.Body))
        {
            body = await reader.ReadToEndAsync();
        }

        if (string.IsNullOrEmpty(body))
        {
            return new BadRequestObjectResult("Request body is required.");
        }

        string storageConnection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        if (string.IsNullOrEmpty(storageConnection))
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        var queueClient = new QueueClient(storageConnection, "salesrequests");
        await queueClient.CreateIfNotExistsAsync();
        await queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(body)));

        return new OkObjectResult("Sales request queued successfully.");
    }
}
