# Deploy Netflicks with Terraform 
A terraform configuration that deploys the deliberately vulnerable Netflicks 
application to an Azure App Service, configured with a Contrast Agent ready for 
demonstrations and evaluations. 

### Creates: 
- A new Resource Group
- A new App Service Plan
- A new App Service
- A new SQL Server
- Some networking / firewall rules
- Pushes the deploy.zip folder from the local machine / scm to the App Service.
- Adds the Contrast .NET Core extension to the App Service.

### How to use: 
```bash
cd terraform/

#
# Edit the demo.auto.tf file to include your Azure Subscription ID, initials and other customizations as required.
#

terraform init
terraform apply
```
