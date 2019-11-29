# Key Vault - Secrets

A Web Api running on an App Service using Key Vault and Managed Service Identity (MSI) to store the database connection string. This is to remove the need to have any secrets in config files.

## Setup

The resources were created with the following CLI commands. The Dev Key Vault is optional unless you are going to start experimenting with different environments. A SQL Database, or even better, and Azure SQL Database, with the AdventureWorks database is required.

Resource names are obviously by choice, but if they need to be globally unique, which they do for Key Vaults and App Services, remember to change the appsettings.Development.json file.

1. ```az group create -l uksouth -n az203-keyvault``` or a location that is better suited.

2. ```az keyvault create -l uksouth -g az203-keyvault -n az203-keyvault``` and ```az keyvault create -l uksouth -g az203-keyvault -n az203-keyvault-dev```

3. ```az keyvault secret set --vault-name az203-keyvault --name "AdventureWorksDbConnectionString" --value "<connection-string-goes-here"``` Substituting your own connection string.

4. ```az appservice plan create --name az203-keyvault-app-plan --resource-group az203-keyvault --sku FREE --is-linux```

5. ```az webapp create --resource-group az203-keyvault --plan az203-keyvault-app-plan --name keyvault-webapi-test --runtime "dotnetcore|3.0"```

6. Deploy the application.

7. Setup MSI and the Key Vault permissions. Notes on this can be found in the links below.

8. Create the application key for `KeyVault__BaseUrl`. Again, notes are in the links below.

## Running
1. Create a get http request to http://<url>/customers, and hopefully a 200 response will come back with 10 customers.

## Clean Up
To save on costs by not having the resources sitting there, run ```az group delete -n <resource group name>``` and confirm when asked.

## Useful stuff
Thanks to the following links:

[Adding KeyVault to an ASP.NET Core application](https://medium.com/@dneimke/add-keyvault-to-an-asp-net-core-application-cab1012d2b60)

[ASP.NET Core + Azure Key Vault + Azure AD MSI = Awesome way to do config](https://joonasw.net/view/aspnet-core-azure-keyvault-msi)
