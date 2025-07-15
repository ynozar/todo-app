/*
resource "google_container_cluster" "primary" {
  name     = "free-tier-cluster"
  location = "us-central1-a"  # Choose an appropriate zone

  initial_node_count = 1
  deletion_protection = false

  node_config {
    machine_type = "e2-micro"  # Free tier eligible instance type
    oauth_scopes = [
      "https://www.googleapis.com/auth/cloud-platform",
    ]
  }

  # Prevent the control plane from auto-scaling
  remove_default_node_pool = true  # This ensures no default pool is created
}

resource "google_container_node_pool" "primary_nodes" {
  name       = "primary-node-pool"
  cluster    = google_container_cluster.primary.name
  location   = google_container_cluster.primary.location

  node_count = 1  # Keep this to 1 to ensure a single node

  node_config {
    machine_type = "e2-micro"  # Ensure it's within free tier
    oauth_scopes = [
      "https://www.googleapis.com/auth/cloud-platform",
    ]
  }
}
*/