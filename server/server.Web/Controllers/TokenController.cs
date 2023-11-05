
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server.Web.Controllers;
[ApiController, Route("api/token")]
public class TokenController : ControllerBase
{
  [HttpPost, Route("refresh")]
  public async Task<IActionResult> Refresh([FromBody] TokenDto token,
    [FromServices] IValidator<TokenDto> tokenValidator,
    [FromServices] ITokenService tokenService,
    [FromServices] ICustomersService customersService)
  {
    var tokenValidatorResult = tokenValidator.Validate(token);
    if (!tokenValidatorResult.IsValid)
      return BadRequest(new { Message = tokenValidatorResult.Errors.First().ErrorMessage });

    ClaimsPrincipal principal = null;
    int customerId;
    try
    {
      principal = tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
      customerId = int.Parse(principal.Identity.Name);
    }
    catch (Exception e)
    {
      Console.WriteLine(e.Message);
      return BadRequest(new { Message = e.Message });
    }

    Customer? customer = await customersService.FindCustomer(c => c.Id == customerId);

    if (customer == null || customer.RefreshToken != token.RefreshToken || 
        customer.RefreshTokenExpiryTime <= DateTime.Now)
      return NotFound(new { Message = "Некорректные данные" });

    string accessToken = tokenService.GenerateAccessToken(principal.Claims);
    string refreshToken = tokenService.GenerateRefreshToken();

    await customersService.SetCustomerRefreshToken(customer, refreshToken);

    return Ok(new TokenDto()
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken
    });
  }
}