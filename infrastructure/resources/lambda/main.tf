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

module "s3" {
  source = "../s3"
}

module "iam" {
  source = "../iam"
}

#region hello_story_api_gateway
resource "aws_lambda_function" "hello_story_api_gateway" {
  function_name = "hello-story-api-gateway"

  s3_bucket = module.s3.aws_s3_bucket_hello_story.id
  s3_key    = module.s3.aws_s3_object_lambda_hello_story_api_gateway.key

  runtime = "dotnet6"
  handler = "HelloStory.APIGatway::HelloStory.APIGatway.Function::Handler"
  memory_size = 256
  timeout = 30

  source_code_hash = module.s3.data_archive_file_lambda_hello_story_api_gateway.output_base64sha256

  role = module.iam.aws_iam_role_lambda_exec_arn
}

resource "aws_lambda_function_url" "hello_story_api_gateway" {
  function_name      = aws_lambda_function.hello_story_api_gateway.function_name
  authorization_type = "NONE"
}
#endregion

#region hello_story_authflow_api
resource "aws_lambda_function" "hello_story_authflow_api" {
  function_name = "hello-story-authflow-api"

  s3_bucket = module.s3.aws_s3_bucket_hello_story.id
  s3_key    = module.s3.aws_s3_object_lambda_hello_story_authflow_api.key

  runtime = "dotnet6"
  handler = "HelloStory.Authflow.API"
  memory_size = 256
  timeout = 30

  source_code_hash = module.s3.data_archive_file_lambda_hello_story_authflow_api.output_base64sha256

  role = module.iam.aws_iam_role_lambda_exec_arn
}

resource "aws_lambda_function_url" "hello_story_authflow_api" {
  function_name      = aws_lambda_function.hello_story_authflow_api.function_name
  authorization_type = "NONE"
}
#endregion