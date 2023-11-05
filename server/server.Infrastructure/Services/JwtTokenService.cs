using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace server.Infrastructure.Services;
public class JwtTokenService : ITokenService
{
  private IConfiguration _configuration;
  public JwtTokenService(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public string GenerateAccessToken(IEnumerable<Claim> claims)
  {
    JwtSecurityToken jwt = new JwtSecurityToken(
      issuer: _configuration["JWT:ISSUER"],
      audience: _configuration["JWT:AUDIENCE"],
      claims: claims,
      expires: DateTime.Now.AddMinutes(15),
      signingCredentials: new SigningCredentials(
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"])),
        SecurityAlgorithms.HmacSha256));

    return new JwtSecurityTokenHandler().WriteToken(jwt);
  }

  public string GenerateRefreshToken()
  {
    byte[] randomNumber = new byte[32];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
  }

  public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = _configuration["JWT:ISSUER"],
        ValidateAudience = true,
        ValidAudience = _configuration["JWT:AUDIENCE"],
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]))
    };

    ClaimsPrincipal principal = new JwtSecurityTokenHandler()
      .ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

    if (securityToken is not JwtSecurityToken jwtSecurityToken ||
        !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        throw new SecurityTokenException("Invalid token");

    return principal;
  }
}
