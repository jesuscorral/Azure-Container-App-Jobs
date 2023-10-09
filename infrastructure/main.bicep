targetScope = 'subscription'

param resourceGroupName string = 'jcp.acajobs'
param location string = 'northeurope'
param prefix string = 'jcp'

var containerAppSufix = '-me'
var logAnalyticsWorkspaceSufix = '-law'
var containerAppEnvironmentName = '${prefix}${containerAppSufix}'
var logAnalyticsWorkspaceName = '${prefix}${logAnalyticsWorkspaceSufix}'

// Resource Group
module rg_resource 'resources/resource-group.bicep' = {
  name: 'resource_group_deployment'
  params: {
    location: location
    resourceGroupName: resourceGroupName
  }
}

module workspace 'resources/log-analytics-workspace.bicep' = {
  name: 'workspace_deployment'
  scope: resourceGroup(resourceGroupName)
  params: {
    name: logAnalyticsWorkspaceName
    location: location
  }
}

module container_app_environment_resource 'resources/container-app-environment.bicep' = {
  name: 'container_app_environment_deployment'
  dependsOn: [rg_resource, workspace]
  scope: resourceGroup(resourceGroupName)
  params: {
    location: location
    name: containerAppEnvironmentName
    workspaceName: workspace.name
  }
}
