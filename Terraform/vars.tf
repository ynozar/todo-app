variable "CERT_PASSWORD" {

description = "The username for the certificate"

type = string
sensitive = true
}


variable "CERTIFICATE" {

description = "Certificate for JWT signing"

type = string
sensitive = true
}

variable "DB_CONNECTION" {

description = "DB Connection string"

type = string

sensitive = true

}


variable "DB_PROD_CONNECTION" {

description = "DB Connection string"

type = string

sensitive = true

}

variable "POSTGRES_USER" {

description = "The password for the database"

type = string

sensitive = false

}


variable "POSTGRES_PASSWORD" {

description = "The password for the database"

type = string
sensitive = true

}


variable "POSTGRES_DB" {

description = "The password for the database"

type = string

}
