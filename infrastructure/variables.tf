variable "issuer" {
  type = string
  default = "Hello Story Authflow API"
  description = "Access token issuer"
}

variable "audience" {
    type = string
    default = ""
    description = "Access token audience"
}

variable "secret_key" {
    type = string
    default = ""
    description = "Access token secret key"
}