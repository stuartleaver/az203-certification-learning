# Azure Web API - Phonebook

An quick example of a Web API. Roughly following the [Host a RESTful API with CORS in Azure App Service](https://docs.microsoft.com/en-gb/azure/app-service/app-service-web-tutorial-rest-api) example but not initially cloning the repo. Instead writing some different code which was a good and bad thing.

## Setup
Instructions are on the link but, to create the resources in Azure using the CLI and Cloud Shell:

1. ```az webapp deployment user set --user-name <username> --password <password>```

2. ```az group create --name <resource-group-name> --location "West Europe"```

3. ```az appservice plan create --name <app-service-plan-name> --resource-group <resource-group-name> --sku FREE --is-linux```

4. ```az webapp create --resource-group <resource-group-name> --plan <app-service-plan-name> --name <app-name> --runtime "ddotnetcore|3.0" --deployment-local-git```

5. On your local machine in a command window, navigate to a working directory.

6. Run ```dotnet new webapi -n <name of app>```

7. Write some code

## Deploying
If creating your own code and with Git installed.
1. ```git init```

2. ```git add .```

3. ```git commit``` and create your commit message.

4. ```git remote add azure <deploymentLocalGitUrl>``` The deployment can either be found from the output of step 4 in the setup or on the overview blade in the Azure Portal.

5. ```git push azure master```

## Running
After its deployed, run some HTTP posts and gets.

A sample post:
URL: - https://<url>/api/Phonebook/
```
{
  "firstName": "Tim",
  "lastName": "Gates",
  "phoneNumber": "(123) 456 7890"
}
```

## Useful stuff
Thanks to the following links:
[Create an Azure App Service plan](https://docs.microsoft.com/en-us/azure/app-service/containers/quickstart-dotnetcore#create-an-azure-app-service-plan)

Thanks to the following CLI command pointing me to Linux
```az webapp list-runtimes --linux | grep DOTNETCORE```
