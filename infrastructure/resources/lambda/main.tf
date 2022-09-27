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

provider "aws" {
  region = "eu-west-2"
}

module "iam" {
  source = "../iam"
}

module "s3" {
  source = "../s3"
}

module "api_gateway" {
  source = "../api-gateway"
}

module "kms" {
  source = "../kms"
}

#region lambdas
#region hello_story_api_gateway_authorizer
resource "aws_lambda_function" "hello_story_api_gateway_authorizer" {
  function_name = "hello-story-api-gateway-authorizer"

  s3_bucket = module.s3.aws_s3_bucket_hello_story.id
  s3_key    = module.s3.aws_s3_object_lambda_hello_story_api_gateway_authorizer.key

  runtime = "dotnet6"
  handler = "HelloStory.APIGatwayAuthorizer::HelloStory.APIGatwayAuthorizer.Function::Handler"
  memory_size = 256
  timeout = 30

  source_code_hash = module.s3.data_archive_file_lambda_hello_story_api_gateway_authorizer.output_base64sha256

  role = module.iam.iam_for_lambda_arn

  environment {
    variables = {
      issuer = var.issuer
      audience = var.audience
      secret_key = var.secret_key
    }
  }

  kms_key_arn = module.kms.aws_kms_key_default_arn
}

resource "aws_lambda_function_url" "hello_story_api_gateway_authorizer" {
  function_name      = aws_lambda_function.hello_story_api_gateway_authorizer.function_name
  authorization_type = "NONE"
}

resource "aws_lambda_permission" "api_gateway" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.hello_story_api_gateway_authorizer.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${module.api_gateway.aws_apigatewayv2_api_hello_story.execution_arn}/*/*"
}

resource "aws_apigatewayv2_integration" "hello_story_api_gateway_authorizer" {
  api_id = module.api_gateway.aws_apigatewayv2_api_hello_story.id

  integration_uri    = aws_lambda_function.hello_story_api_gateway_authorizer.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
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

  role = module.iam.iam_for_lambda_arn
}

resource "aws_lambda_function_url" "hello_story_authflow_api" {
  function_name      = aws_lambda_function.hello_story_authflow_api.function_name
  authorization_type = "NONE"
}

resource "aws_lambda_permission" "api_authflow" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.hello_story_authflow_api.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${module.api_gateway.aws_apigatewayv2_api_hello_story.execution_arn}/*/*"
}

resource "aws_apigatewayv2_integration" "hello_story_api_authflow" {
  api_id = module.api_gateway.aws_apigatewayv2_api_hello_story.id

  integration_uri    = aws_lambda_function.hello_story_authflow_api.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

#region routes
resource "aws_apigatewayv2_route" "hello_story_authflow_api_sign_in" {
  api_id    = module.api_gateway.aws_apigatewayv2_api_hello_story.id
  route_key = "POST /authflow/sign-in"
  target    = "integrations/${aws_apigatewayv2_integration.hello_story_api_authflow.id}"
}

resource "aws_apigatewayv2_route" "hello_story_authflow_api_refresh_token" {
  api_id    = module.api_gateway.aws_apigatewayv2_api_hello_story.id
  route_key = "POST /authflow/token/refresh"
  target    = "integrations/${aws_apigatewayv2_integration.hello_story_api_authflow.id}"
}

resource "aws_apigatewayv2_route" "hello_story_authflow_api_revoke_token" {
  api_id    = module.api_gateway.aws_apigatewayv2_api_hello_story.id
  route_key = "POST /authflow/token/revoke"
  target    = "integrations/${aws_apigatewayv2_integration.hello_story_api_authflow.id}"
}

#endregion
#endregion
#region hello_story_user_api
resource "aws_lambda_function" "hello_story_user_api" {
  function_name = "hello-story-user-api"

  s3_bucket = module.s3.aws_s3_bucket_hello_story.id
  s3_key    = module.s3.aws_s3_object_lambda_hello_story_user_api.key

  runtime = "dotnet6"
  handler = "HelloStory.Authflow.API"
  memory_size = 256
  timeout = 30

  source_code_hash = module.s3.data_archive_file_lambda_hello_story_user_api.output_base64sha256

  role = module.iam.iam_for_lambda_arn
}

resource "aws_lambda_function_url" "hello_story_user_api" {
  function_name      = aws_lambda_function.hello_story_user_api.function_name
  authorization_type = "NONE"
}

resource "aws_lambda_permission" "api_user" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.hello_story_user_api.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${module.api_gateway.aws_apigatewayv2_api_hello_story.execution_arn}/*/*"
}

resource "aws_apigatewayv2_integration" "hello_story_api_user" {
  api_id = module.api_gateway.aws_apigatewayv2_api_hello_story.id

  integration_uri    = aws_lambda_function.hello_story_user_api.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

#region routes
resource "aws_apigatewayv2_route" "hello_story_api_user_sign_up" {
  api_id    = module.api_gateway.aws_apigatewayv2_api_hello_story.id
  route_key = "POST /user/sign-up"
  target    = "integrations/${aws_apigatewayv2_integration.hello_story_api_user.id}"
}
#endregion
#endregion