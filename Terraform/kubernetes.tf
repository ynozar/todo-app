/*
resource "kubernetes_secret" "example" {
  metadata {
    name      = "my-secret"
    namespace = "default"
  }

  data = {
    DB_PROD_CONNECTION = var.DB_PROD_CONNECTION
  }

  type = "Opaque"
}
*/