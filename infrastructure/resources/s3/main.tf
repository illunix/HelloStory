terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.31.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.1.0"
    }
    archive = {
      source  = "hashicorp/archive"
      version = "~> 2.2.0"
    }
  }

  required_version = "~> 1.0"
}

data "archive_file" "lambda_hello_story_authflow_api" {
  type = "zip"

  source_dir  = "${path.module}/../../../src/api/Features/Authflow/HelloStory.Authflow.API/bin/Release/net6.0/linux-x64"
  output_path = "${path.module}/../../../src/api/Features/Authflow/HelloStory.Authflow.API/bin/Release/net6.0/linux-x64.zip"
}

resource "aws_s3_object" "lambda_hello_story_authflow_api" {
  bucket = aws_s3_bucket.hello_story_authflow_api.id

  key    = "hello-story-authflow-api.zip"
  source = data.archive_file.lambda_hello_story_authflow_api.output_path

  etag = filemd5(data.archive_file.lambda_hello_story_authflow_api.output_path)
}

resource "random_pet" "lambda_bucket_name" {
  prefix = "hello-story-authflow-api"
  length = 4
}

resource "aws_s3_bucket" "hello_story_authflow_api" {
  bucket = random_pet.lambda_bucket_name.id
}