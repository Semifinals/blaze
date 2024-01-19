targetScope = 'subscription'

// Parameters
@description('The environment to build in')
@allowed([
  'dev'
  'prod'
])
param environment string

@description('The location to deploy to')
param location string

// Names
var product = 'blaze'
var suffix = '-sf-${product}-${environment}'
var suffixNoDash = 'sf${product}${environment}'

var resourceGroup = 'rg${suffix}'
var cosmosDb = 'db${suffix}'
var applicationInsights = 'ai${suffix}'
var serverFarm = 'sf${suffix}'
var storageAccount = 'sa${suffixNoDash}'
var functionApp = 'fa${suffix}'

// Create resource group
resource azResourceGroup 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroup
  location: location
}

// Create cosmos db
module azCosmosDb 'modules/azCosmosDb.bicep' = {
  scope: azResourceGroup
  name: 'cosmosDb'
  params: {
    name: cosmosDb
    location: location
  }
}

// Create application insights
module azApplicationInsights 'modules/azApplicationInsights.bicep' = {
  scope: azResourceGroup
  name: 'applicationInsights'
  params: {
    name: applicationInsights
    location: location
  }
}

// Create server farm
module azServerFarm 'modules/azServerFarm.bicep' = {
  scope: azResourceGroup
  name: 'serverFarm'
  params: {
    name: serverFarm
    location: location
  }
}

// Create storage account
module azStorageAccount 'modules/azStorageAccount.bicep' = {
  scope: azResourceGroup
  name: 'storageAccount'
  params: {
    name: storageAccount
    location: location
  }
}

// Create function app
module azFunctionApp 'modules/azFunctionApp.bicep' = {
  scope: azResourceGroup
  name: 'functionApp'
  params: {
    name: functionApp
    location: location
    serverFarmId: azServerFarm.outputs.id
    storageName: azStorageAccount.outputs.name
    cosmosDbName: azCosmosDb.outputs.name
    applicationInsightsInstrumentationKey: azApplicationInsights.outputs.instrumentationKey
  }
}

// Outputs
output resourceGroupName string = azResourceGroup.name
output functionAppName string = azFunctionApp.outputs.name
