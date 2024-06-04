
# Create a resource group
resource "azurerm_resource_group" "todoRG" {
  name     = "todo-app"
  location = "eastus"
}

resource "azurerm_log_analytics_workspace" "example" {
  name                = "acctest"
  location            = azurerm_resource_group.hello.location
  resource_group_name = azurerm_resource_group.hello.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_container_app_environment" "todoEnv" {
  name                       = "ToDo-Environment"
  location                   = azurerm_resource_group.hello.location
  resource_group_name        = azurerm_resource_group.hello.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.example.id
}

resource "azurerm_container_app" "backend" {
  name                         = "backend"
  container_app_environment_id = azurerm_container_app_environment.example.id
  resource_group_name          = azurerm_resource_group.hello.name
  revision_mode                = "Single"

  template {
    container {
      name   = "backend"
      image  = "ynozar/todo-api:latest"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}

resource "azurerm_container_app" "frontend" {
  name                         = "ui"
  container_app_environment_id = azurerm_container_app_environment.example.id
  resource_group_name          = azurerm_resource_group.hello.name
  revision_mode                = "Single"

  template {
    container {
      name   = "ui"
      image  = "ynozar/todo-ui:latest"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}


resource "azurerm_container_app" "database" {
  name                         = "db"
  container_app_environment_id = azurerm_container_app_environment.example.id
  resource_group_name          = azurerm_resource_group.hello.name
  revision_mode                = "Single"

  template {
    container {
      name   = "db"
      image  = "postgres:latest"
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}

