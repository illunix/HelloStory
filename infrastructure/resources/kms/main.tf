terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.31.0"
    }
    archive = {
      source  = "hashicorp/archive"
    }
  }

  required_version = "~> 1.0"
}

resource "aws_kms_key" "default" {
  description             = "KMS key 1"
}