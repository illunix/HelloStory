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

resource "random_pet" "lambda_bucket_name" {
  prefix = "hello-story-lambdas"
  length = 4
}

resource "aws_s3_bucket" "hello_story" {
  bucket = random_pet.lambda_bucket_name.id
}

#region hello_story_api_gateway_authorizer
data "archive_file" "lambda_hello_story_api_gateway_authorizer" {
  type = "zip"

  source_dir  = "${path.module}/../../../src/api/Gateway/HelloStory.APIGatway/bin/Release/net6.0/linux-x64"
  output_path = "${path.module}/../../../src/api/Gateway/HelloStory.APIGateway/bin/Release/net6.0/linux-x64.zip"
}

resource "aws_s3_object" "lambda_hello_story_api_gateway_authorizer" {
  bucket = aws_s3_bucket.hello_story.id

  key    = "hello-story-api-gateway.zip"
  source = data.archive_file.lambda_hello_story_api_gateway_authorizer.output_path

  etag = filemd5(data.archive_file.lambda_hello_story_api_gateway_authorizer.output_path)
}
#endregion

#region hello_story_authflow_api
data "archive_file" "lambda_hello_story_authflow_api" {
  type = "zip"

  source_dir  = "${path.module}/../../../src/api/Features/Authflow/HelloStory.Authflow.API/bin/Release/net6.0/linux-x64"
  output_path = "${path.module}/../../../src/api/Features/Authflow/HelloStory.Authflow.API/bin/Release/net6.0/linux-x64.zip"
}

resource "aws_s3_object" "lambda_hello_story_authflow_api" {
  bucket = aws_s3_bucket.hello_story.id

  key    = "hello-story-authflow-api.zip"
  source = data.archive_file.lambda_hello_story_authflow_api.output_path

  etag = filemd5(data.archive_file.lambda_hello_story_authflow_api.output_path)
}
#endregion