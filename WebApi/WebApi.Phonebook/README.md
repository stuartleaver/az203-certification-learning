# Azure Web API - Phonebook

An quick example of a Web API. Roughly following the [Host a RESTful API with CORS in Azure App Service](https://docs.microsoft.com/en-gb/azure/app-service/app-service-web-tutorial-rest-api) example but not initially cloning the repo. Instead writing some different code which was a good and bad thing.

## Setup
Instructions are on the link but, to create the resources in Azure using the CLI and Cloud Shell:

1. ```az webapp deployment user set --user-name <username> --password <password>```

2. ```az group create --name myResourceGroup --location "West Europe"```

3. ```az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku FREE```

4. ```az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name <app-name> --deployment-local-git```

5. On your local machine in a command window, navigate to a working directory.

6. Run ```dotnet new webapi -n <name of app>``` This is where it fell apart a bit.

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


## Issues
1. It doesn't seem that webapi is a supported project as an error was given saying the csproj file type was not supported. I had more (if you can call it that) luck using mvc in step 6 in the setup.

2. It doesn't seem that Web Apps in Azure support 3.0 yet - https://stackoverflow.com/questions/58203359/azure-web-app-with-net-core-3-0-fails-cant-find-microsoft-aspnetcore-app Trying the Extension didn't work either. Maybe it was something I was doing wrong.

If anyone wants to help fix the above issues, please feel free.
