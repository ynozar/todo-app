variable "CERT_PASSWORD" {

description = "The username for the certificate"
  default = ""
type = string
sensitive = true
}

variable "PROJECT_ID" {

description = "The GCP project_id"

type = string
}

variable "CERTIFICATE" {

description = "Certificate for JWT signing"

type = string
  default = ""
sensitive = true
}

variable "DB_CONNECTION" {

description = "DB Connection string"
  default = ""
type = string

sensitive = true

}


variable "DB_PROD_CONNECTION" {

description = "DB Connection string"
  default = ""
type = string

sensitive = true

}

variable "region" {
  type = string
  default = "us-central1-a"
}

variable "COMMIT_HASH" {
  type = string
  default = "latest"
}
