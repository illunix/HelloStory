using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloStory.APIGatway;

internal sealed class Function
{
    public APIGatewayCustomAuthorizerResponse Handler(
        APIGatewayCustomAuthorizerRequest req,
        ILambdaContext ctx
    )
    {
        var claimsPrincipal = () =>
        {
            return new JwtSecurityTokenHandler().ValidateToken(
                req.Headers["authorization"],
                new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("issuer"),

                    ValidateAudience = true,
                    ValidAudience = Environment.GetEnvironmentVariable("audience"),

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("secretKey")!)),

                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out var securityToken
            );
        };

        return new()
        {
            PrincipalID = claimsPrincipal() is null ? "401" : claimsPrincipal()?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            PolicyDocument = new()
            {
                Statement = new()
                {
                    new()
                    {
                        Effect = claimsPrincipal() is null ? "Deny" : "Allow",
                        Resource = new HashSet<string> { "arn:aws:execute-api:ap-south-1:821175633958:sctmtm1ge8/*/*" },
                        Action = new HashSet<string> { "execute-api:Invoke" }
                    }
                }
            }
        };
    }
}
