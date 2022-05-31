param containerAppName string
param environmentId string
param containerImage string
param containerPort int
param isExternalIngress bool = false
param containerRegistry string
param containerRegistryUsername string
param minReplicas int = 0
param maxReplicas int = 10
param environmentVars array = []
param location string = resourceGroup().location

@secure()
param containerRegistryPassword string

resource containerApp 'Microsoft.App/containerApps@2022-01-01-preview' = {
  name: containerAppName
  location: location
  properties: {
    managedEnvironmentId: environmentId
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
      activeRevisionsMode: 'single'
      ingress: {
        external: isExternalIngress
        targetPort: containerPort
      }
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
