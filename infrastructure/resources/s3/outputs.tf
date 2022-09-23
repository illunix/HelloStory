output "data_archive_file_lambda_hello_story_api_gateway" {
  value = data.archive_file.lambda_hello_story_api_gateway
}

output "aws_s3_object_lambda_hello_story_api_gateway" {
  value = aws_s3_object.lambda_hello_story_api_gateway
}

output "data_archive_file_lambda_hello_story_authflow_api" {
  value = data.archive_file.lambda_hello_story_authflow_api
}

output "aws_s3_object_lambda_hello_story_authflow_api" {
  value = aws_s3_object.lambda_hello_story_authflow_api
}

output "aws_s3_bucket_hello_story" {
  value = aws_s3_bucket.hello_story
}