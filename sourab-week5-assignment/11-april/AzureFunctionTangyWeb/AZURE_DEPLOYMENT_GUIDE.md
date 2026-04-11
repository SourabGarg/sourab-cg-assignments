# Universal Azure Deployment Guide (.NET)

This guide is a reusable checklist for deploying:
- `ASP.NET Core MVC`
- `ASP.NET Core Web API`
- `Azure Functions` (isolated worker, .NET)

It supports both **free/low-cost** and **production-ready** setups.

---

## 1) Prerequisites

Install and verify:
- [.NET SDK 8+](https://dotnet.microsoft.com/download)
- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli)
- [Git](https://git-scm.com/downloads)
- (Optional) [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local)

Login:

```powershell
az login
az account show --output table
```

If you have multiple subscriptions:

```powershell
az account set --subscription "<SUBSCRIPTION_ID_OR_NAME>"
```

---

## 2) Cost-aware hosting choices

### MVC / Razor Pages / Web API
- **Free tier**: App Service Plan `F1` (good for demos/testing)
- **Low-cost paid**: `B1` (better reliability and features)

### Azure Functions
- **Lowest practical cost**: Consumption plan (`Y1`) with monthly free grant
- Use one Function App per workload for simpler scaling/isolation

### Shared resources
- Storage Account: `Standard_LRS`
- Monitoring: Application Insights (recommended)

---

## 3) Standard project readiness checks

Run before deploy:

```powershell
dotnet restore
dotnet build -c Release
```

If tests exist:

```powershell
dotnet test
```

### Configuration best practices
- Never hardcode secrets.
- Put runtime settings in Azure App Settings.
- Use environment variables in code (example: `FunctionApiBaseUrl`).
- Prefer Azure Key Vault for secrets in real environments.

---

## 4) Deploy ASP.NET Core MVC or Web API (App Service)

## Variables

```powershell
$rg = "rg-myapp-dev"
$loc = "eastus2"
$plan = "asp-myapp-free"
$web = "web-myapp-<unique>"
```

## Create infrastructure

```powershell
az group create --name $rg --location $loc
az appservice plan create --name $plan --resource-group $rg --location $loc --sku F1
az webapp create --resource-group $rg --plan $plan --name $web
az webapp config set --resource-group $rg --name $web --net-framework-version v8.0
```

## Publish app

```powershell
dotnet publish .\<YourWebProject>.csproj -c Release -o .\publish\web
Compress-Archive -Path .\publish\web\* -DestinationPath .\publish\web.zip -Force
az webapp deploy --resource-group $rg --name $web --src-path .\publish\web.zip --type zip
```

## Configure app settings

```powershell
az webapp config appsettings set --resource-group $rg --name $web --settings \
  "ASPNETCORE_ENVIRONMENT=Production" \
  "FunctionApiBaseUrl=https://<your-function-app>.azurewebsites.net/api/"
```

---

## 5) Deploy Azure Function (.NET isolated)

## Variables

```powershell
$rg = "rg-myapp-dev"
$loc = "eastus2"
$stg = "stmyapp<unique>"
$func = "func-myapp-<unique>"
```

## Create infrastructure

```powershell
az group create --name $rg --location $loc
az storage account create --name $stg --resource-group $rg --location $loc --sku Standard_LRS
az functionapp create --resource-group $rg --consumption-plan-location $loc --name $func --storage-account $stg --runtime dotnet-isolated --runtime-version 8 --functions-version 4 --os-type Windows
```

## Configure app settings

```powershell
az functionapp config appsettings set --name $func --resource-group $rg --settings \
  "FUNCTIONS_WORKER_RUNTIME=dotnet-isolated" \
  "AzureSqlDatabase=<SQL_CONNECTION_STRING>" \
  "AzureWebJobsStorage=<STORAGE_CONNECTION_STRING_IF_REQUIRED>"
```

## Publish function

```powershell
dotnet publish .\<YourFunctionProject>.csproj -c Release -o .\publish\function
Compress-Archive -Path .\publish\function\* -DestinationPath .\publish\function.zip -Force
az functionapp deployment source config-zip --resource-group $rg --name $func --src .\publish\function.zip
```

## Validate function exists

```powershell
az functionapp function list --resource-group $rg --name $func --output table
```

---

## 6) Connect Web App -> Function endpoint

If your MVC/Web API calls a function endpoint:

1. Read base URL from environment variable in code.
2. Set it in Web App settings:

```powershell
az webapp config appsettings set --resource-group $rg --name $web --settings "FunctionApiBaseUrl=https://$func.azurewebsites.net/api/"
```

3. Restart app after config updates:

```powershell
az webapp restart --resource-group $rg --name $web
```

---

## 7) Database and queue-dependent apps

For apps like `Web -> Function HTTP -> Queue -> Queue Trigger -> SQL`:

- Ensure SQL firewall allows Azure services / correct IPs.
- Ensure queue names are lowercase.
- Ensure Function app has correct storage and DB settings.
- Verify DB schema exists before running queue consumers.

---

## 8) Verification checklist

### Web app

```powershell
az webapp show --resource-group $rg --name $web --query "defaultHostName" -o tsv
```

Open:
`https://<webapp-name>.azurewebsites.net`

### Function app

```powershell
az functionapp show --resource-group $rg --name $func --query "defaultHostName" -o tsv
az functionapp function list --resource-group $rg --name $func --output table
```

### Logs

```powershell
az webapp log tail --resource-group $rg --name $web
az functionapp logstream --resource-group $rg --name $func
```

---

## 9) CI/CD option (recommended)

Use GitHub Actions with:
- `azure/webapps-deploy` for MVC/Web API
- `Azure/functions-action` for Functions

Store secrets in repository/environment secrets:
- `AZURE_CREDENTIALS`
- publish profiles or federated identity setup

---

## 10) Common issues

- **500 on web app**: missing app settings / wrong runtime / startup error.
- **Function not triggered**: wrong queue name or storage setting.
- **Function HTTP 401/403**: function auth level/key mismatch.
- **DB insert failures**: bad connection string or SQL firewall blocked.
- **App calling localhost in Azure**: replace with env-based URL config.

---

## 11) Security checklist

- Remove secrets from `appsettings.json` / `local.settings.json` in source control.
- Rotate exposed keys/passwords immediately.
- Use Managed Identity + Key Vault where possible.
- Enforce HTTPS only.
- Keep `Minimum TLS` at `1.2`.

---

## 12) Cleanup to avoid charges

Delete everything in one step:

```powershell
az group delete --name $rg --yes --no-wait
```

---

## Quick template summary

- MVC/Web API: App Service (`F1` for free demo)
- Function: Consumption plan (`Y1`)
- Use env variables for all endpoints and secrets
- Publish with `dotnet publish` + zip deploy
- Validate endpoints and logs immediately after deployment
