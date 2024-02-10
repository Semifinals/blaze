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
    "CosmosConnectionString": "YOUR_COSMOS_DB_EMULATOR_CONNECTION_STRING",

    "OpenApi__ApiKey": "YOUR_API_KEY_TO_SECURE_OPENAPI_ENDPOINTS",
    "OpenApi__AuthLevel__Document": "Anonymous",
    "OpenApi__AuthLevel__UI": "Anonymous",
    "OpenApi__HideSwaggerUI": false,
    "OpenApi__HideDocument": false,
  
    "OpenApi__Version": "v2",
    "OpenApi__DocTitle": "Semifinals Blaze",
    "OpenApi__DocDescription": "A URL shortener microservice to redirect from short links to the rest of the internet",
    "OpenApi__DocVersion": "1.0.0",
    "OpenApi__ForceHttps": false,
    "OpenApi__ForceHttp": true
  }
}
```
