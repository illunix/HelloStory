resource "aws_apigatewayv2_api" "hello_story" {
  name          = "hello-story-api-gateway"
  protocol_type = "HTTP"
}