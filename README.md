# Microsoft Orleans on Azure Container Apps sample

I needed to know if it was possible to run Orleans on Azure Container Apps. It is and this is a very simple sample that demonstrates that

It is a bit rough around the edges, maybe I'll clean it up a bit more later

Currently deployed here: https://spio-hello-aca4-client.orangegrass-26d615b9.northeurope.azurecontainerapps.io/

## Build the docker images

Build the docker images and push them to a docker registry

## Use Azure Bicep to deploy it

I am very new to [Azure Bicep](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/) so I used this to learn a bit about that

The Bicep files are ing the `./deploy` folder

In the  `main.bicep` there is a number of parameters needed, you'll probably figure it out.
