terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.31.0"
    }
  }

  required_version = "~> 1.0"
}

provider "aws" {
  region = "eu-west-2"
}

resource "aws_apigatewayv2_api" "hello_story" {
  name          = "hello-story-api-gateway"
  protocol_type = "HTTP"
}

resource "aws_apigatewayv2_stage" "api" {
  api_id = aws_apigatewayv2_api.hello_story.id
  name   = "api"
  auto_deploy = true
}