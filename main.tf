#Terraform `provider` section is required since the `azurerm` provider update to 2.0+
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
resource "azurerm_sql_server" "app" {
  name                         = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-sql-server"
  resource_group_name          = azurerm_resource_group.personal.name
  location                     = azurerm_resource_group.personal.location
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = random_password.password.result
}

#Set up a database
resource "azurerm_sql_database" "app" {
  name                = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-sql-database"
  resource_group_name = azurerm_resource_group.personal.name
  location            = azurerm_resource_group.personal.location
  server_name         = azurerm_sql_server.app.name
}

#Set up a firewall rule
resource "azurerm_sql_firewall_rule" "database" {
  name                = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-firewall-rule"
  resource_group_name = azurerm_resource_group.personal.name
  server_name         = azurerm_sql_server.app.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

#Set up an app service plan
resource "azurerm_app_service_plan" "app" {
  name                = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-serviceplan"
  location            = azurerm_resource_group.personal.location
  resource_group_name = azurerm_resource_group.personal.name

  sku {
    tier = "Standard"
    size = "S1"
  }
}

#Set up an app service
resource "azurerm_app_service" "app" {
  name                = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-app-service"
  location            = azurerm_resource_group.personal.location
  resource_group_name = azurerm_resource_group.personal.name
  app_service_plan_id = azurerm_app_service_plan.app.id

  site_config {
    always_on = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"                    = "Development"
    "ConnectionStrings__DotNetFlicksConnection" = "Server=tcp:${azurerm_sql_server.app.name}.database.windows.net,1433;Initial Catalog=${azurerm_sql_database.app.name};Persist Security Info=False;User ID=${azurerm_sql_server.app.administrator_login};Password=${azurerm_sql_server.app.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    "CORECLR_ENABLE_PROFILING"                  = "1"
    "CORECLR_PROFILER"                          = "{8B2CE134-0948-48CA-A4B2-80DDAD9F5791}"
    "CORECLR_PROFILER_PATH_32"                  = "D:\\home\\SiteExtensions\\Contrast.NetCore.Azure.SiteExtension\\ContrastNetCoreAppService\\runtimes\\win-x32\\native\\ContrastProfiler.dll"
    "CORECLR_PROFILER_PATH_64"                  = "D:\\home\\SiteExtensions\\Contrast.NetCore.Azure.SiteExtension\\ContrastNetCoreAppService\\runtimes\\win-x32\\native\\ContrastProfiler.dll"
    "CONTRAST_DATA_DIRECTORY"                   = "D:\\home\\SiteExtensions\\Contrast.NetCore.Azure.SiteExtension\\ContrastNetCoreAppService\\runtimes\\win-x32\\native\\"
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
    "CONTRAST__AGENT__LOGGER__LEVEL"          = "INFO"
    "CONTRAST__AGENT__LOGGER__ROLL_DAILY"     = "true"
    "CONTRAST__AGENT__LOGGER__BACKUPS"         = "30"
  }

  provisioner "local-exec" {
    command     = "./deploy.sh"
    working_dir = path.module

    environment = {
      webappname        = "${replace(var.appname, "/[^-0-9a-zA-Z]/", "-")}-${var.initials}-app-service"
      resourcegroupname = azurerm_resource_group.personal.name
    }
  }
}

resource "null_resource" "before" {
  depends_on = [azurerm_app_service.app]
}

resource "null_resource" "delay" {
  provisioner "local-exec" {
    command = "sleep 40"
  }
  triggers = {
    "before" = "${null_resource.before.id}"
  }
}

resource "null_resource" "after" {
  depends_on = [null_resource.delay]
}

resource "azurerm_template_deployment" "extension" {
  name                = "extension"
  resource_group_name = azurerm_app_service.app.resource_group_name
  template_body       = file("siteextensions.json")

  parameters = {
    "siteName"          = azurerm_app_service.app.name
    "extensionName"     = "Contrast.NetCore.Azure.SiteExtension"   

  }

  deployment_mode     = "Incremental"
  #wait until the app service starts before installing the extension
  depends_on = [null_resource.delay]

  #restart the app service after installing the extension
  provisioner "local-exec" {
    command     = "az webapp restart --name ${azurerm_app_service.app.name} --resource-group ${azurerm_app_service.app.resource_group_name}"      
  }

}

resource "null_resource" "restart" {
  provisioner "local-exec" {
    command     = "az webapp restart --name ${azurerm_app_service.app.name} --resource-group ${azurerm_app_service.app.resource_group_name}"      
  }
  triggers = {
    "before" = "${azurerm_template_deployment.extension.id}"
  }
}
