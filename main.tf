#Terraform `provider` section is required since the `azurerm` provider update to 2.0+
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.29.0"
    }
  }

}

provider "azurerm" {
  features {

  }
}

#Extract the connection from the normal yaml file to pass to the app container
data "external" "yaml" {
  program = [var.python_binary, "${path.module}/parseyaml.py"]
}

#Set up a personal resource group for the SE local to them
resource "azurerm_resource_group" "personal" {
  name     = "Sales-Engineer-${var.initials}"
  location = var.location
}

resource "random_password" "password" {
  length           = 25
  special          = true
  override_special = "_%@"
}

#Set up a sql server
resource "azurerm_mssql_server" "app" {
  name                         = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-sql-server"
  resource_group_name          = azurerm_resource_group.personal.name
  location                     = azurerm_resource_group.personal.location
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = random_password.password.result
}

#Set up a database
resource "azurerm_mssql_database" "app" {
  name      = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-sql-database"
  server_id = azurerm_mssql_server.app.id
}

#Set up a firewall rule
resource "azurerm_mssql_firewall_rule" "database" {
  name             = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-firewall-rule"
  server_id        = azurerm_mssql_server.app.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

#Set up an app service plan
resource "azurerm_service_plan" "app" {
  name                = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-serviceplan"
  location            = azurerm_resource_group.personal.location
  resource_group_name = azurerm_resource_group.personal.name
  os_type             = "Windows"
  sku_name            = "S1"
}

resource "azurerm_windows_web_app" "app" {
  name                = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-app-service"
  location            = azurerm_resource_group.personal.location
  resource_group_name = azurerm_resource_group.personal.name
  service_plan_id     = azurerm_service_plan.app.id
  zip_deploy_file     = "./deploy/deploy.zip"

  site_config {
    always_on = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"                    = "Development"
    "ConnectionStrings__DotNetFlicksConnection" = "Server=tcp:${azurerm_mssql_server.app.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.app.name};Persist Security Info=False;User ID=${azurerm_mssql_server.app.administrator_login};Password=${azurerm_mssql_server.app.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    "CONTRAST__API__URL"                        = data.external.yaml.result.url
    "CONTRAST__API__USER_NAME"                  = data.external.yaml.result.user_name
    "CONTRAST__API__SERVICE_KEY"                = data.external.yaml.result.service_key
    "CONTRAST__API__API_KEY"                    = data.external.yaml.result.api_key
    "CONTRAST__APPLICATION__NAME"               = var.appname
    "CONTRAST__SERVER__NAME"                    = var.servername
    "CONTRAST__SERVER__ENVIRONMENT"             = var.environment
    "CONTRAST__APPLICATION__SESSION_METADATA"   = var.session_metadata
    "CONTRAST__SERVER__TAGS"                    = var.servertags
    "CONTRAST__APPLICATION__TAGS"               = var.apptags
    "CONTRAST__AGENT__LOGGER__LEVEL"            = "INFO"
    "CONTRAST__AGENT__LOGGER__ROLL_DAILY"       = "true"
    "CONTRAST__AGENT__LOGGER__BACKUPS"          = "30"
  }
}

resource "azurerm_resource_group_template_deployment" "extension" {
  name                = "extension"
  resource_group_name = azurerm_windows_web_app.app.resource_group_name

  template_content = file("siteextensions.json")

  parameters_content = jsonencode({
    "siteName" = {
      value = azurerm_windows_web_app.app.name
    }
    "extensionName" = {
      value = "Contrast.NetCore.Azure.SiteExtension"
    }
  })

  deployment_mode = "Incremental"
}
