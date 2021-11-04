param resourcesBaseName string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: '${resourcesBaseName}-logs'
  location: resourceGroup().location
  properties: any({
    retentionInDays: 30
    features: {
      searchVersion: 1
    }
    sku: {
      name: 'PerGB2018'
    }
  })
}

resource containerAppEnv 'Microsoft.Web/kubeEnvironments@2021-02-01' = {
  name: '${resourcesBaseName}-env'
  location: resourceGroup().location
  kind: 'containerenvironment'
  properties: {
    type: 'managed'
    internalLoadBalancerEnabled: false
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
  }
}

output environmentId string = containerAppEnv.id
