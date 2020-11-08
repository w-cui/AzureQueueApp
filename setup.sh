#!/bin/bash

RESOURCE_GROUP=[your-resource-group]
STORAGE_ACCOUNT_NAME=storage$(openssl rand -hex 5)

az storage account create \
--name $STORAGE_ACCOUNT_NAME \
-g $RESOURCE_GROUP \
--kind StorageV2 \
--sku Standard_LRS

dotnet new console -n AzureQueueApp

cd AzureQueueApp

az storage account show-connection-string \
--name $STORAGE_ACCOUNT_NAME \
--resource-group $RESOURCE_GROUP > appsettings.json

dotnet add package WindowsAzure.Storage
