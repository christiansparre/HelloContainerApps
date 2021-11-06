param containerRegistry string
param containerRegistryUsername string
param environmentVars array = []
param containerAppBaseName string
param siloImageName string
param clientImageName string

@secure()
param containerRegistryPassword string

targetScope = 'subscription'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: '${containerAppBaseName}-rg'
  location: deployment().location
}

module containerAppEnvDeploy 'container-app-environment.bicep' = {
  name: 'containerAppEnvDeploy'
  scope: rg
  params: {
    containerAppBaseName: containerAppBaseName
  }
}

module storageAccountDeploy 'container-app-storage-account.bicep' = {
  name: 'storageAccountDeploy'
  scope: rg
}

var envVars = [
  {
    'name': 'AzureStorageConnectionString'
    'value': storageAccountDeploy.outputs.storageConnectionString
  }
  {
    'name': 'AzureStorageTableName'
    'value': 'orleanscluster${uniqueString(rg.id)}'
  }
  {
    'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
    'value': containerAppEnvDeploy.outputs.appInsightsInstrumentationKey
  }
]

module siloContainerAppDeploy 'container-app-silo.bicep' = {
  name: 'siloContainerAppDeploy'
  scope: rg
  params: {
    environmentId: containerAppEnvDeploy.outputs.environmentId
    containerRegistry: containerRegistry
    containerRegistryUsername: containerRegistryUsername
    containerImage: siloImageName
    containerRegistryPassword: containerRegistryPassword
    environmentVars: union(environmentVars, envVars)
    containerAppName: '${containerAppBaseName}-silo'
    minReplicas: 3
  }
}

module clientContainerAppDeploy 'container-app-client.bicep' = {
  name: 'clientContainerAppDeploy'
  scope: rg
  params: {
    environmentId: containerAppEnvDeploy.outputs.environmentId
    containerRegistry: containerRegistry
    containerRegistryUsername: containerRegistryUsername
    containerImage: clientImageName
    containerRegistryPassword: containerRegistryPassword
    environmentVars: union(environmentVars, envVars)
    containerAppName: '${containerAppBaseName}-client'
    isExternalIngress: true
    containerPort: 80
    minReplicas: 1
    maxReplicas: 1
  }
}
