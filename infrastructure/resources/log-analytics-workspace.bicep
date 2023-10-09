// Parameters
@description('Specifies the name of the Log Analytics workspace.')
param name string

@description('Specifies the location.')
param location string = resourceGroup().location

var retentionInDays = 60
var sku = 'PerNode'

// Resources
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: name
  location: location
  properties: {
    sku: {
      name: sku
    }
    retentionInDays: retentionInDays
  }
}

//Outputs
output id string = logAnalyticsWorkspace.id
output name string = logAnalyticsWorkspace.name
output customerId string = logAnalyticsWorkspace.properties.customerId
