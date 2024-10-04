# Reserve a Global IP Address
/*
resource "google_compute_global_address" "lb_ip" {
  name = "global-load-ip"
}
# URL Map
resource "google_compute_url_map" "default" {
  name = "global-load-url-map"

  default_service = google_compute_backend_service.default.id

  # If you want more advanced routing, you can define multiple host or path rules here
}
# HTTP Proxy
resource "google_compute_target_http_proxy" "default" {
  name    = "global-load-url-http-proxy"
  url_map = google_compute_url_map.default.id
}

# Global Forwarding Rule
resource "google_compute_global_forwarding_rule" "default" {
  name       = "global-load-url-forwarding-rule"
  target     = google_compute_target_http_proxy.default.id
  port_range = "80"
}
resource "google_compute_backend_service" "default" {
  name        = "load-backend"
  protocol    = "HTTPS"
  backend {
    group = google_cloud_run_service.todo_api.status[0].url
  }
  }

*/