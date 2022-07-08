# Netflicks: A deliberately insecure .NET Core web application

This is a .NET Core 3.1 demo application, based on https://github.com/LeviHassel/.net-flicks with added vulnerabilities. 

**Warning**: The computer running this application will be vulnerable to attacks, please take appropriate precautions.

The credentials for admin access are admin@dotnetflicks.com / p@ssWORD471

# Running in Docker

You can run Netflicks within a Docker container, tested on OSX. It uses a separate sql server as specified within docker-compose.yml (you should not need to edit this file). The agent is added automatically during the Docker build process.

1. Place a .NET specific `contrast_security.yaml` file into the application's root folder.
1. Build the Netflicks container image using `./image.sh`
1. Run the containers using `docker-compose up`

# Running in Azure (Azure App Service):

## Pre-Requisites

1. Place a .NET specific `contrast_security.yaml` file into the application's root folder.
1. Install Terraform from here: https://www.terraform.io/downloads.html.
1. Install PyYAML using `pip install PyYAML`.
1. Install the Azure cli tools using `brew update && brew install azure-cli`.
1. Log into Azure to make sure you cache your credentials using `az login`.
1. Edit the [variables.tf](variables.tf) file (or add a terraform.tfvars) to add your initials, preferred Azure location, app name, server name and environment.
1. Run `terraform init` to download the required plugins.
1. Run `terraform plan` and check the output for errors.
1. Run `terraform apply` to build the infrastructure that you need in Azure, this will output the web address for the application. If you receive a HTTP 503 error when visiting the app then wait 30 seconds for the application to initialize.
1. Run `terraform destroy` when you would like to stop the app service and release the resources.

The terraform file will automatically add the Contrast .NET Azure site extension, so you will always get the latest version.

# Running automated tests

There is a test script which you can use to reveal all the vulnerabilities which requires node and puppeteer.

1. Install Node, NPM and Chrome.
1. From the app folder run `npm i puppeteer`.
1. Run `BASEURL=https://<your service name>.azurewebsites.net node exercise.js` or `BASEURL=https://<your service name>.azurewebsites.net DEBUG=true node exercise.js` to watch the automated script.

# Deploying a new version

If you change the application you should run the Publish option in Visual Studio which will update the content in deploy folder. Zip this up into an archive called deploy.zip. 

# Vulnerabilities

1. SQL Injection from "Search" Parameter on "/Movie" page
1. Cross-Site Scripting from "Email" Parameter on "/Account/ForgotPasswordConfirmation" page
1. XML External Entity Injection (XXE) from "Biography" Parameter on "/Person/Edit/{n}" page
1. Path Traversal from "Email" Parameter on "/Account/Login" page
1. Path Traversal from "PhoneNumber" Parameter on "/Manage/Index" page
1. OS Command Injection from "Email" Parameter on "/Account/Login" page