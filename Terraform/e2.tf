/*
resource "google_compute_instance" "e2_micro" {
  name         = "my-e2-micro-instance"
  machine_type = "e2-micro"
  zone         = "us-central1-a"  # Change to your desired zone

  boot_disk {
    initialize_params {
       image = "ubuntu-os-cloud/ubuntu-2204-lts"
    }
  }

  network_interface {
    network = "default"
    access_config {
      // Ephemeral public IP
    }
  }

  metadata = {
    enable-oslogin = "true"  # Optional: Enable OS Login
  }
}

*/
