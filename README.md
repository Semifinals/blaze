# Blaze

A URL shortener microservice to redirect from short links to the rest of the internet. Built in F# running on the in-process .NET functions runtime.

## Configuration

To run locally, create a `./Blaze/local.settings.json` file with the following content:

```json
{
  "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "CosmosConnectionString": "YOUR_COSMOS_DB_EMULATOR_CONNECTION_STRING"
    }
}
```
