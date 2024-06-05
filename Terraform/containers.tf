

resource "azurerm_log_analytics_workspace" "logs" {
  name                = "acctest"
  location            = azurerm_resource_group.todoRG.location
  resource_group_name = azurerm_resource_group.todoRG.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_container_app_environment" "todoEnv" {
  name                       = "ToDo-Environment"
  location                   = azurerm_resource_group.todoRG.location
  resource_group_name        = azurerm_resource_group.todoRG.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.logs.id
}


resource "azurerm_container_app" "frontend" {
  name                         = "ui"
  container_app_environment_id = azurerm_container_app_environment.todoEnv.id
  resource_group_name          = azurerm_resource_group.todoRG.name
  revision_mode                = "Single"

ingress {
    allow_insecure_connections = false
    external_enabled = true
    target_port = 80
    transport = "auto"
    traffic_weight {
      percentage = 100
      latest_revision = true
    }
  }



  template {
    min_replicas = 1
    max_replicas = 2
    container {
      name   = "ui"
      image  = "ynozar/todo-ui" #removed :latest
      cpu    = 0.25
      memory = "0.5Gi"
    }
  }
}



resource "azurerm_container_app" "database" {
  name                         = "db"
  container_app_environment_id = azurerm_container_app_environment.todoEnv.id
  resource_group_name          = azurerm_resource_group.todoRG.name
  revision_mode                = "Single"

  ingress {
    allow_insecure_connections = false
    external_enabled = false
    exposed_port = 5432
    target_port = 5432
    transport = "tcp"
    traffic_weight {
      percentage = 100
      latest_revision = true
    }
  }


  template {
    min_replicas = 1
    max_replicas = 1
    container {
      name   = "db"
      image  = "postgres:latest"
      cpu    = 0.25
      memory = "0.5Gi"
  
  
 
  env {
    name = "POSTGRES_USER"
    value = var.POSTGRES_USER
  }
  env {
    name = "POSTGRES_PASSWORD"
    value = var.POSTGRES_PASSWORD
  }
  env {
    name = "POSTGRES_DB"
    value = var.POSTGRES_DB
  }

    }
    
  }


}


resource "azurerm_container_app" "backend" {
  name                         = "backend"
  container_app_environment_id = azurerm_container_app_environment.todoEnv.id
  resource_group_name          = azurerm_resource_group.todoRG.name
  revision_mode                = "Single"

ingress {
    allow_insecure_connections = false
    external_enabled = true
    target_port = 8080
    transport = "auto"
    traffic_weight {
      percentage = 100
      latest_revision = true
    }
  }





  template {
    min_replicas = 1
    max_replicas = 2
    container {
      name   = "backend"
      image  = "ynozar/todo-api" #removed :latest
      cpu    = 0.5
      memory = "1Gi"

      env {
          name = "DB_CONNECTION"
          value = var.DB_PROD_CONNECTION
        }

      env {
        name = "CERT_PASSWORD"
        value = var.CERT_PASSWORD
      }

      env {
        name = "CERTIFICATE"
        value = var.CERTIFICATE
     }
    }
  }
}