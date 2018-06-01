variable "subscription_id" {}
variable "container" {}
variable "functionname" {}

provider "azurerm" {
  subscription_id = "${var.subscription_id}"
  version         = "~> 1.6"
}

provider "random" {
  version = "~> 1.3"
}

resource "azurerm_resource_group" "etlpipe" {
  name     = "${var.functionname}"
  location = "northeurope"
}

resource "random_id" "etlpipe" {
  keepers = {
    # Generate a new ID only when a new resource group is defined
    resource_group = "${azurerm_resource_group.etlpipe.name}"
  }

  byte_length = 2
}

resource "azurerm_storage_account" "etlpipe" {
  name                     = "etlpipe${random_id.etlpipe.dec}"
  resource_group_name      = "${azurerm_resource_group.etlpipe.name}"
  location                 = "${azurerm_resource_group.etlpipe.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "etlpipe" {
  name                  = "${var.container}"
  resource_group_name   = "${azurerm_resource_group.etlpipe.name}"
  storage_account_name  = "${azurerm_storage_account.etlpipe.name}"
  container_access_type = "container"
}

resource "azurerm_app_service_plan" "etlpipe" {
  name                = "azure-functions-etlpipe-service-plan"
  location            = "${azurerm_resource_group.etlpipe.location}"
  resource_group_name = "${azurerm_resource_group.etlpipe.name}"
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "etlpipe" {
  name                      = "${var.functionname}${random_id.etlpipe.dec}"
  location                  = "${azurerm_resource_group.etlpipe.location}"
  resource_group_name       = "${azurerm_resource_group.etlpipe.name}"
  app_service_plan_id       = "${azurerm_app_service_plan.etlpipe.id}"
  storage_connection_string = "${azurerm_storage_account.etlpipe.primary_connection_string}"
}

data "azurerm_storage_account" "etlpipe" {
  name                = "${azurerm_storage_account.etlpipe.name}"
  resource_group_name = "${azurerm_resource_group.etlpipe.name}"
}

output "file_endpoint" {
  value = "${data.azurerm_storage_account.etlpipe.primary_file_endpoint}"
}

output "function_name" {
  value = "${azurerm_function_app.etlpipe.name}"
}

output "account_name" {
  value = "${data.azurerm_storage_account.etlpipe.name}"
}

output "primary_connection_string" {
  value = "${data.azurerm_storage_account.etlpipe.primary_connection_string}"
}
