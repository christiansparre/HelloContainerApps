param containerAppName string
param environmentId string
param containerImage string
param containerRegistry string
param containerRegistryUsername string
param minReplicas int = 0
param maxReplicas int = 10
param environmentVars array = []

@secure()
param containerRegistryPassword string

resource containerApp 'Microsoft.Web/containerApps@2021-03-01' = {
  name: containerAppName
  kind: 'containerapp'
  location: resourceGroup().location
  properties: {
    kubeEnvironmentId: environmentId
    configuration: {
      secrets: [
        {
          name: 'registry-password'
          value: containerRegistryPassword
        }
      ]      
      registries: [
        {
          server: containerRegistry
          username: containerRegistryUsername
          passwordSecretRef: 'registry-password'
        }
      ]    
    }
    template: {
      containers: [
        {
          image: containerImage
          name: containerAppName
          env: environmentVars
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
      }
    }
  }
}
