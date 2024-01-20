// Parameters
param name string
param location string

// Create cosmos db
resource azCosmosDb 'Microsoft.DocumentDB/databaseAccounts@2023-04-15' = {
  name: name
  kind: 'GlobalDocumentDB'
  location: location
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
  }
}

// Create links-db database
resource azCosmosDbDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-11-15' = {
  parent: azCosmosDb
  name: 'links-db'
  location: location
  properties: {
    resource: {
      id: 'links-db'
    }
  }
}

// Create links container
resource azCosmosDbContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-11-15' = {
  parent: azCosmosDbDatabase
  name: 'links'
  location: location
  properties: {
    resource: {
      id: 'links'
      partitionKey: {
        paths: ['/id']
        kind: 'Hash'
      }
    }
  }
}

// Outputs
output name string = azCosmosDb.name
