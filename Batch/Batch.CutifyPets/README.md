# Azure Batch Cutify Pets

A .NET Core console app to perform video transcoding, in parallel, with Azure Batch services. This is from the [Create an app to run parallel compute jobs in Azure Batch](https://docs.microsoft.com/en-us/learn/modules/create-an-app-to-run-parallel-compute-jobs-in-azure-batch/) module on [Microsoft Learn](https://docs.microsoft.com/en-us/learn/).

## Setup
1. The link to the module will run through creating the required resources. However, most is also possible through the Azure CLI to add a bit of fun!
  * Create a Resource Group ```az group create --name <resource group name> --location <location>```

  * Create a Storage Account ```az storage account create --name <storage account name> --location <location> --resource-group <resource group name> --sku Standard_LRS --kind StorageV2```

  * Create a Batch account ```az batch account create --location <location> --name <batch account name> --storage-account <storage account name> --resource-group <resource group name>```
  
  * An application package needs creating but that was done manually!

2. In `Program.cs`, enter your Batch and Storage account credentials which can be found on the Keys (Batch) and Access Keys (Storage Account) blades of the resources.
```cs
// Batch account credentials
private const string batchAccountUrl = "BATCH_URL";
private const string batchAccountName = "BATCH_NAME";
private const string batchAccountKey = "BATCH_KEY";
// Storage account credentials
private const string storageAccountName = "STORAGE_NAME";
private const string storageAccountKey = "STORAGE_KEY";
```

## Running
1. Run the application and read the module to see what is happening!.

2. Press any key to exit.

## Clean Up
To save on costs by not having the resources sitting there, run ```az group delete -n <resource group name>``` and confirm when asked.

## Notes
If an error occurs due to the Job existing already, you can delete it with ```az batch job delete --job-id WinFFmpegJob --account-name <batch account name> --account-key <batch key> --account-endpoint <batch endpoint>```
