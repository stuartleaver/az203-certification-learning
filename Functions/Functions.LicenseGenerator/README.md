# Azure Web API - Phonebook

An quick example of a Azure Function. Roughly following the [Pluralsight - Azure Functions Fundamentals](https://app.pluralsight.com/library/courses/azure-functions-fundamentals/table-of-contents) example.

## The concept

1. Accept a post HttpTrigger with a body like the following and store it is a queue.
```json
{
	"OrderId": 123456,
	"ProductId": "MY-FIRST-FUNCTION",
	"Email": "joe-blogs@unknown.com"
}
```
2. Using the QueueTrigger, create the license file and store in a Blob output.

3. Using the BlobTrigger, send the email using SendGrid.

## Setup
The resources can be created in multiple ways but using the CLI is fun! To create the resources in Azure using the CLI and Cloud Shell:

1. ```az group create --name <resource-group-name> --location <location>```

2. ```az storage account create --name <storage-account-name> --location <location> --resource-group <resource-group-name> --sku Standard_LRS```

3. ```az functionapp create --resource-group <resource-group-name> --consumption-plan-location <location> --name <function-app-name> --storage-account <storage-account-name> --runtime dotnet```

5. On your local machine, create a Function app project. There are multiple ways of doing this but I used VS Code. I just use the code here.

6. Set the different values in your ```local.settings.json``` file.

7. Run the project and using the URL, create a post request with a body like the example above.

## Deploying
1. Just publish the project to the Function app in Azure.

## Running
Run the project to get a local URL or use the published version in Azure. Send a post HTTP request like the one above.

## Useful stuff
[Quickstart: Create your first function from the command line using Azure CLI](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-first-azure-function-azure-cli)
