using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;
using server.Infrastructure.Validation;
using System.Security.Claims;

namespace server.Web.Controllers;

[ApiController, Route("api/customers")]
public class CustomersController : ControllerBase
{
  private ICustomersService _customersService;
  public CustomersController(ICustomersService customersService) =>
    _customersService = customersService;


  // @desc Register customer
  // @route POST api/customers/reg
  [HttpPost, Route("reg")]
  public async Task<IActionResult> Register([FromBody] RegisteredCustomerDto registeredCustomer,
    [FromServices] IValidator<RegisteredCustomerDto> registeredCustomerValidator)
  {
    var registeredCustomerValidatorResult = registeredCustomerValidator.Validate(registeredCustomer);
    if (!registeredCustomerValidatorResult.IsValid)
      return BadRequest(new { Message = registeredCustomerValidatorResult.Errors.First().ErrorMessage });

    if (await _customersService.FindCustomer(registeredCustomer.Email) != null)
      return BadRequest(new { Message = "Пользователь с данным email уже существует" });

    await _customersService.AddCustomer(registeredCustomer);

    return Ok(new 
    {
      FirstName = registeredCustomer.FirstName,
      LastName = registeredCustomer.LastName,
      Email = registeredCustomer.Email
    });
  }

  // @desc Login customer
  // @route POST api/customers/login
  [HttpPost, Route("login")]
  public async Task<IActionResult> Login([FromBody] LoginCustomerDto loginCustomer,
    IValidator<LoginCustomerDto> loginCustomerValidator, 
    [FromServices] ITokenService tokenService)
  {
    var loginCustomerValidatorResult = loginCustomerValidator.Validate(loginCustomer);
    if (!loginCustomerValidatorResult.IsValid)
      return BadRequest(new { Message = loginCustomerValidatorResult.Errors.First().ErrorMessage });

    Customer? customer = await _customersService.FindCustomer(loginCustomer);

    if (customer == null)
      return NotFound(new { Message = "Пользователь не найден" });

    var claims = new List<Claim> 
    {
      new Claim(ClaimTypes.Name, customer.Id.ToString()),
      new Claim(ClaimTypes.Role, (await _customersService.GetCustomerRole(customer)).Name)
    };
    string accessToken = tokenService.GenerateAccessToken(claims);

    string refreshToken = tokenService.GenerateRefreshToken();
    await _customersService.SetCustomerRefreshToken(customer, refreshToken);

    return Ok(new TokenDto()
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken
    });
  }

  // @desc Get customer profile
  // @route GET api/customers
  [HttpGet, Authorize]
  public async Task<IActionResult> GetCustomerProfile()
  {
    int customerId = int.Parse(User.Identity.Name);

    Customer? customer = await _customersService.FindCustomer(c => c.Id == customerId);

    if (customer == null) return NotFound(new { Message = "Пользователь не найден" });

    return Ok(new CustomerDto()
    {
      FirstName = customer.FirstName,
      LastName = customer.LastName,
      BirthDate = customer.BirthDate,
      Phone = customer.Phone,
      Address = customer.Address,
      City = customer.City,
      Email = customer.Email,
      Balance = customer.Balance,
      Points = customer.Points
    });
  }

  // @desc Change customer
  // @route PUT api/customers
  [Authorize, HttpPut]
  public async Task<IActionResult> ChangeCustomer([FromBody] ChangedCustomerDto changedCustomer,
    [FromServices] IValidator<ChangedCustomerDto> changedCustomerValidator)
  {
    var changedCustomerValidatorResult = changedCustomerValidator.Validate(changedCustomer);
    if (!changedCustomerValidatorResult.IsValid)
      return BadRequest(new { Message = changedCustomerValidatorResult.Errors.First().ErrorMessage });

    int customerId = int.Parse(User.Identity.Name);
    Customer? customer = await _customersService.FindCustomer(c => c.Id == customerId);

    if (customer == null)
      return NotFound(new { Message = "Пользователь не найден" });

    await _customersService.ChangeCustomer(customer, changedCustomer);

    return Ok(new { Message = "Ваш аккаунт успешно изменён" });
  }

  // @desc Delete customer
  // @route DELETE api/customers
  [Authorize, HttpDelete]
  public async Task<IActionResult> DeleteCustomer()
  {
    int customerId = int.Parse(User.Identity.Name);
    Customer? customer = await _customersService.FindCustomer(c => c.Id == customerId);

    if (customer == null) return NotFound(new { Message = "Пользователь не найден" });

    await _customersService.DeleteCustomer(customer);

    return Ok(new { Message = "Вы успешно удалили свой аккаунт" });
  }

  // @desc Get public customer info
  // @route GET api/customers/{id:int}
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetPublicCustomerInfo(int id) =>
    Ok((await _customersService.FindCustomer(c => c.Id == id))?.FirstName);
}