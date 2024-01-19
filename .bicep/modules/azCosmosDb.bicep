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

// Outputs
output name string = azCosmosDb.name
