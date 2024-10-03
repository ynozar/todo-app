
resource "google_cloud_run_service" "todo_api" {
  name     = "todo-api"
  location = var.region # Change as necessary

  template {
    spec {
      containers {
        image = "ynozar/todo-api:${var.COMMIT_HASH}"

        ports {
          container_port = 8080
        }
        env {
          name = "DB_CONNECTION"
          value = var.DB_PROD_CONNECTION
        }
        env {
          name = "RSA_PRIVATE"
          value = var.RSA_PRIVATE
        }
      }
    }
  }

  autogenerate_revision_name = true
}

resource "google_cloud_run_service_iam_member" "todo_api_invoker" {
  service      = google_cloud_run_service.todo_api.name
  location     = google_cloud_run_service.todo_api.location
  role         = "roles/run.invoker"
  member       = "allUsers"
}


resource "google_cloud_run_service" "todo_ui" {
  name     = "todo-ui"
  location = var.region # Change as necessary

  template {
    spec {
      containers {
        image = "ynozar/todo-ui:${var.COMMIT_HASH}"

        ports {
          container_port = 80
        }
      }
    }
  }

  autogenerate_revision_name = true
}

resource "google_cloud_run_service_iam_member" "todo_ui_invoker" {
  service      = google_cloud_run_service.todo_ui.name
  location     = google_cloud_run_service.todo_ui.location
  role         = "roles/run.invoker"
  member       = "allUsers"
}

output "api_url" {
  value = google_cloud_run_service.todo_api.traffic[0].url
}


output "ui_url" {
  value = google_cloud_run_service.todo_ui.traffic[0].url
}