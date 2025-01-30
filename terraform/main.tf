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

#Set up a personal resource group for the SE local to them
resource "azurerm_resource_group" "personal" {
  name     = "Sales-Engineer-${var.initials}-${var.instance_name}-${var.contrast_application_name}"
  location = var.location
}

resource "random_password" "password" {
  length           = 25
  special          = true
  override_special = "_%@"
}

#Set up a sql server
resource "azurerm_mssql_server" "app" {
  name                         = "${var.initials}-${var.instance_name}-${replace(var.contrast_application_name, "/[^-0-9a-zA-Z]/", "-")}-sql-server"
  resource_group_name          = azurerm_resource_group.personal.name
  location                     = azurerm_resource_group.personal.location
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = random_password.password.result
}

#Set up a database
resource "azurerm_mssql_database" "app" {
  name      = "${var.initials}-${var.instance_name}-${replace(var.contrast_application_name, "/[^-0-9a-zA-Z]/", "-")}-sql-database"
  server_id = azurerm_mssql_server.app.id
}

#Set up a firewall rule
resource "azurerm_mssql_firewall_rule" "database" {
  name             = "${var.initials}-${var.instance_name}-${replace(var.contrast_application_name, "/[^-0-9a-zA-Z]/", "-")}-firewall-rule"
  server_id        = azurerm_mssql_server.app.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

#Set up an app service plan
resource "azurerm_service_plan" "app" {
  name                = "${var.initials}-${var.instance_name}-${replace(var.contrast_application_name, "/[^-0-9a-zA-Z]/", "-")}-service-plan"
  location            = azurerm_resource_group.personal.location
  resource_group_name = azurerm_resource_group.personal.name
  os_type             = "Windows"
  sku_name            = "B3"
}

resource "azurerm_windows_web_app" "app" {
  name                = "${var.initials}-${var.instance_name}-${replace(var.contrast_application_name, "/[^-0-9a-zA-Z]/", "-")}-app"
  location            = azurerm_resource_group.personal.location
  resource_group_name = azurerm_resource_group.personal.name
  service_plan_id     = azurerm_service_plan.app.id
  zip_deploy_file     = "../deploy/deploy.zip"

  site_config {
    always_on = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"                    = "Development"
    "ConnectionStrings__DotNetFlicksConnection" = "Server=tcp:${azurerm_mssql_server.app.name}.database.windows.net,1433;Initial Catalog=${azurerm_mssql_database.app.name};Persist Security Info=False;User ID=${azurerm_mssql_server.app.administrator_login};Password=${azurerm_mssql_server.app.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    "CONTRAST__API__TOKEN"                      = var.contrast_agent_token
    "CONTRAST__APPLICATION__NAME"               = var.contrast_application_name
    "CONTRAST__SERVER__NAME"                    = var.contrast_server_name
    "CONTRAST__SERVER__ENVIRONMENT"             = var.contrast_environment
    "CONTRAST__APPLICATION__SESSION_METADATA"   = var.contrast_session_metadata
    "CONTRAST__SERVER__TAGS"                    = var.contrast_server_tags
    "CONTRAST__APPLICATION__TAGS"               = var.contrast_application_tags
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
