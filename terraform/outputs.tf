output "fqdn" {
  value = "https://${azurerm_windows_web_app.app.default_hostname}"
}

output "contrast" {
  value = "This app should appear in Contrast TeamServer"
}

