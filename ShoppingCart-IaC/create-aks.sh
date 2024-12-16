#!/usr/bin/env bash

az provider register --namespace Microsoft.ContainerService
az provider register --namespace Microsoft.Compute
az provider register --namespace Microsoft.Network
az provider register --namespace Microsoft.Storage 
 
az provider show --namespace Microsoft.ContainerService -o table
az provider show --namespace Microsoft.Compute -o table
az provider show --namespace Microsoft.Network -o table
az provider show --namespace Microsoft.Storage -o table

# Variables (customize these)
RESOURCE_GROUP="ecomm-rg"
CLUSTER_NAME="ecomm-clstr"
LOCATION="indiacentral"
NODE_COUNT=1            # A small node count for dev
NODE_SIZE="Standard_B2s" # A small VM size to keep costs low

# Create a resource group
az group create \
  --name $RESOURCE_GROUP \
  --location $LOCATION

# Create a basic AKS cluster
az aks create \
  --resource-group $RESOURCE_GROUP \
  --name $CLUSTER_NAME \
  --node-count $NODE_COUNT \
  --node-vm-size $NODE_SIZE \
  --generate-ssh-keys \
  --attach-acr ""  # Remove if you don't have ACR or leave empty

# Once created, get credentials to interact with your cluster
az aks get-credentials \
  --resource-group $RESOURCE_GROUP \
  --name $CLUSTER_NAME



az aks get-credentials --resource-group ecomm-rg --name ecomm-clstr --file ~/.kube/config --overwrite-existing

az aks show --resource-group ecomm-rg --name ecomm-clstr --output json

#stop the cluster
az aks scale --resource-group ecomm-rg --name ecomm-clstr --node-count 0
#start the cluster
az aks scale --resource-group ecomm-rg --name ecomm-clstr --node-count 1

#check the consumption
az consumption usage list --start-date $(date +%Y-%m-%d) --end-date $(date +%Y-%m-%d)
# --query "[].{Name:instanceName, Cost:pretaxCost, ResourceType:product}" \
#   --output table


# Output the current context to verify the cluster access
kubectl config get-contexts
