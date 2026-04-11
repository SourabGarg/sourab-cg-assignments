# AzureFunctionTangyWeb + TangyAzureFunc (.NET 8)

## Project Overview
This solution implements a simple end-to-end workflow:

1. User submits a form in the web app (`AzureFunctionTangyWeb`).
2. The web app sends a JSON payload to an Azure Function HTTP trigger.
3. The function writes the payload to Azure Storage Queue.
4. A second queue-triggered function reads the message and saves it to SQL (`SalesRequests` table).

---

## Tech Stack
- `.NET 8`
- ASP.NET Core MVC (`AzureFunctionTangyWeb`)
- Azure Functions Isolated Worker (`TangyAzureFunc`)
- Azure Storage Queue
- Azure SQL Database
- Entity Framework Core 8 (`SqlServer`)
- `Newtonsoft.Json`

---

## Solution Structure
- `AzureFunctionTangyWeb/`
  - `Controllers/HomeController.cs`
  - `Models/SalesRequest.cs`
  - `Program.cs`
- `TangyAzureFunc/`
  - `OnSalesUploadWriteToQueue.cs`
  - `OnQueueTriggerUpdateDatabase.cs`
  - `Data/ApplicationDbContext.cs`
  - `Models/SalesRequest.cs`
  - `Program.cs`
  - `local.settings.json`

---

## Data Flow
1. `POST /Home/Index` receives form model.
2. `HomeController` generates `Id` and posts JSON to:
   - `http://localhost:7023/api/OnSalesUploadWriteToQueue`
3. `OnSalesUploadWriteToQueue` returns model via:
   - `[QueueOutput("salesrequestoutbound", Connection = "AzureWebJobsStorage")]`
4. `OnQueueTriggerUpdateDatabase` listens on:
   - `[QueueTrigger("salesrequestoutbound", Connection = "AzureWebJobsStorage")]`
5. Function inserts data into SQL table `dbo.SalesRequests`.

> Queue name is lowercase (`salesrequestoutbound`) because Azure queue names must be lowercase.

---

## Required Configuration
Set values in `TangyAzureFunc/local.settings.json`:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "<storage-connection-string>",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "AzureSqlDatabase": "<sql-connection-string>"
  }
}
```

### SQL Table
Create table in target database:

```sql
CREATE TABLE [dbo].[SalesRequests] (
    [Id] [nvarchar](450) NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Email] [nvarchar](max) NOT NULL,
    [Phone] [nvarchar](max) NOT NULL,
    [Status] [nvarchar](max) NOT NULL,
    CONSTRAINT [PK_SalesRequests] PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

---

## NuGet Packages Used
### `AzureFunctionTangyWeb`
- `Newtonsoft.Json`

### `TangyAzureFunc`
- `Microsoft.Azure.Functions.Worker`
- `Microsoft.Azure.Functions.Worker.Sdk`
- `Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore`
- `Microsoft.Azure.Functions.Worker.Extensions.Storage.Queues`
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Newtonsoft.Json`

---

## How to Run (Visual Studio)
1. Open solution.
2. Set **Multiple startup projects**:
   - `AzureFunctionTangyWeb` = `Start`
   - `TangyAzureFunc` = `Start`
3. Ensure function port in `TangyAzureFunc/Properties/launchSettings.json` is:
   - `--port 7023`
4. Run solution.
5. Open web app page and submit form.

---

## How to Verify
### Function Console Logs
Expected sequence:
- `OnSalesUploadWriteToQueue` succeeded.
- `OnQueueTriggerUpdateDatabase` triggered by `salesrequestoutbound`.
- EF `INSERT INTO [SalesRequests]` executed.

### SQL Validation
```sql
SELECT TOP 20 * FROM dbo.SalesRequests ORDER BY Id DESC;
```

### Storage Queue Validation (Azure CLI)
```powershell
az storage queue exists --name salesrequestoutbound --account-name <account> --account-key <key>
az storage message peek --queue-name salesrequestoutbound-poison --account-name <account> --account-key <key> --num-messages 5
```

---

## Troubleshooting
### Login failed for SQL user
- Validate username/password in `AzureSqlDatabase`.
- Check Azure SQL firewall rules.
- Test with `sqlcmd`:

```powershell
sqlcmd -S "tcp:<server>.database.windows.net,1433" -d "<db>" -U "<user>" -P "<password>" -N -C -Q "SELECT 1"
```

### Function not reachable from web app
- Confirm function is running at `http://localhost:7023`.
- Ensure MVC base address matches function port.

### Messages moving to poison queue
- Queue trigger is failing repeatedly (often DB connection/auth).
- Fix root cause, then clear poison queue if needed.

---

## Security Notes
- Do **not** commit secrets in `local.settings.json`.
- Rotate exposed storage keys and SQL passwords.
- Prefer secret management (`User Secrets`, `Key Vault`, environment variables) for real deployments.

---

## Current Status
The complete pipeline (Web -> HTTP Function -> Queue -> Queue Trigger -> Azure SQL) is implemented and working once valid SQL credentials are configured.
