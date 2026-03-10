# .NET 8 LTS Web App Sample

A sample ASP.NET Core 8 LTS web application ready for deployment to Azure Web App.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli) (optional, for CLI deployment)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

## Local Development

### 1. Clone the repository

```bash
git clone https://github.com/SecretiveRabbit/web-app-deploy.git
cd web-app-deploy
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Run the application

```bash
dotnet run
```

The application will start on:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001

### 4. Access the API

- **Swagger UI**: https://localhost:5001/swagger
- **Weather API**: https://localhost:5001/api/weatherforecast

## Project Structure

```
.
├── WebAppDeploy.csproj          # Project configuration
├── Program.cs                    # Application entry point
├── appsettings.json             # Production settings
├── appsettings.Development.json # Development settings
├── Controllers/
│   └── WeatherForecastController.cs
├── Models/
│   └── WeatherForecast.cs
└── README.md
```

## Deployment to Azure Web App

### Option 1: Deploy using Azure Portal

1. **Create an Azure Web App:**
   - Go to [Azure Portal](https://portal.azure.com)
   - Create a new Web App resource
   - Select **Runtime stack**: `.NET 8 (LTS)`
   - Select **Operating System**: Windows or Linux
   - Configure the App Service Plan (e.g., Free tier for testing)

2. **Publish from Visual Studio:**
   - Right-click on the project → **Publish**
   - Select **Azure** as target
   - Sign in with your Azure account
   - Select your Web App
   - Click **Publish**

### Option 2: Deploy using Azure CLI

```bash
# Login to Azure
az login

# Create a resource group
az group create --name myResourceGroup --location eastus

# Create an App Service Plan
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku FREE --is-linux

# Create a Web App
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name myWebApp --runtime "DOTNET|8.0"

# Publish the application
dotnet publish -c Release -o ./publish
cd publish
zip -r ../app.zip .
az webapp deployment source config-zip --resource-group myResourceGroup --name myWebApp --src ../app.zip
```

### Option 3: Deploy using GitHub Actions

1. Create `.github/workflows/azure-deploy.yml`:

```yaml
name: Deploy to Azure Web App

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Build
        run: dotnet build --configuration Release
      
      - name: Publish
        run: dotnet publish -c Release -o ./publish
      
      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'myWebApp'
          publish-profile: ${{ secrets.AZURE_PUBLISHPROFILE }}
          package: './publish'
```

2. Add your Azure publish profile as a GitHub secret (`AZURE_PUBLISHPROFILE`)

## Configuration

### appsettings.json

Modify `appsettings.json` to configure:
- Logging levels
- Connection strings
- Custom application settings

### Environment Variables

In Azure Web App, set application settings via:
- **Azure Portal** → Web App → Settings → Configuration
- Or use Azure CLI: `az webapp config appsettings set`

Example:
```bash
az webapp config appsettings set --resource-group myResourceGroup --name myWebApp --settings "MyKey=MyValue"
```

## Monitoring

### Application Insights

1. Create an Application Insights resource in Azure
2. Get the instrumentation key
3. Add to `appsettings.json `:

```json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-key-here"
  }
}
```

4. Install NuGet package:
```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

5. Update `Program.cs`:
```csharp
builder.Services.AddApplicationInsightsTelemetry();
```

## Troubleshooting

### Application won't start
- Check Azure Portal → Web App → Log Stream for errors
- Ensure .NET 8 runtime is selected
- Verify all dependencies are included in publish

### Connection timeout
- Check Azure Firewall/NSG rules
- Verify the Web App is running (check status in Portal)
- Review application logs

### Performance issues
- Use Application Insights to diagnose
- Consider upgrading the App Service Plan
- Enable caching and compression

## Learning Resources

- [ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core/)
- [Azure Web App Documentation](https://learn.microsoft.com/azure/app-service/)
- [Azure .NET Developer Guide](https://learn.microsoft.com/dotnet/azure/)
- [Swagger/OpenAPI](https://learn.microsoft.com/aspnet/core/tutorials/web-api-help-pages-using-swagger)

## License

MIT