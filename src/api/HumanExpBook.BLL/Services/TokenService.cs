using HumanExpBook.BLL.Interfaces;
using HumanExpBook.Common.Options;
using HumanExpBook.Common.Security;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HumanExpBook.BLL.Services;

public sealed partial class TokenService : ITokenService
{
    private readonly IOptions<JwtOptions> _options;

    public string GenerateAccessToken(string userId)
        => new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    _options.Value.Issuer,
                    _options.Value.Audience,
                    new Claim[] {
                        new(
                            ClaimTypes.NameIdentifier,
                            userId
                        ),
                        new(
                            JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString()
                        )
                    },
                    _options.Value.NotBefore,
                    _options.Value.Expiration,
                    _options.Value.SigningCredentials
        ));

    public string GenerateRefreshToken()
        => Convert.ToBase64String(SecurityHelper.GetRandomBytes());
}