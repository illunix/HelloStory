terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.31.0"
    }
  }

  required_version = "~> 1.0"
}

module "rds" {
  source = "./resources/rds"
}

module "lambda" {
  source = "./resources/lambda"
  issuer = var.issuer
  audience = var.audience
  secret_key = var.secret_key
}