variable "initials" {
  description = "Enter your initials to include in URLs. Lowercase only!!!"
  default     = ""
}

variable "location" {
  description = "The Azure location where all resources in this example should be created, to find your nearest run `az account list-locations -o table`"
  default     = ""
}

variable "appname" {
  description = "The name of the app to display in Contrast TeamServer. Also used for DNS, so no spaces please!"
  default     = "netflicks"
}

variable "servername" {
  description = "The name of the server to display in Contrast TeamServer."
  default     = "netflicks-app-service"
}

variable "environment" {
  description = "The Contrast environment for the app. Valid values: development, qa or production"
  default     = "development"
}

variable "session_metadata" {
  description = "See https://docs.contrastsecurity.com/user-vulnerableapps.html#session"
  default     = ""
}

variable "python_binary" {
  description = "Path to local Python binary"
  default     = "python"
}

variable "apptags" {
  description = "Tags to be associated with the app in Contrast TeamServer."
  default     = ""
}

variable "servertags" {
  description = "Tags to be associated with the server in Contrast TeamServer."
  default     = ""
}

