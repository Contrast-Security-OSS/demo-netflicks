variable "initials" {
  description = "Enter your initials to include in URLs. Lowercase only!!!"
  type = string
}

variable instance_name {
  description = "A unique name for this instance of the app. Typically the project or TeamServer organisation the agent will report to."
  type = string
}

variable "location" {
  description = "The Azure location where all resources in this example should be created, to find your nearest run `az account list-locations -o table`"
  type = string
}

variable "contrast_agent_token" {
  description = "The API token for the Contrast agent to communicate back to Contrast TeamServer."
  type = string
  sensitive = true
}

variable "contrast_application_name" {
  description = "The name of the app to display in Contrast TeamServer. Also used for DNS, so no spaces please!"
  type = string
  default     = "demo-netflicks"
}

variable "contrast_server_name" {
  description = "The name of the server to display in Contrast TeamServer."
  type = string
  default     = "azure-netflicks-app-service"
}

variable "contrast_environment" {
  description = "The Contrast environment for the app. Valid values: development, qa or production"
  type = string
  default     = "development"
}

variable "contrast_session_metadata" {
  description = "See https://docs.contrastsecurity.com/user-vulnerableapps.html#session"
  type = string
  default     = ""
}

variable "contrast_application_tags" {
  description = "Tags to be associated with the app in Contrast TeamServer."
  type = string
  default     = ""
}

variable "contrast_server_tags" {
  description = "Tags to be associated with the server in Contrast TeamServer."
  type = string
  default     = ""
}

