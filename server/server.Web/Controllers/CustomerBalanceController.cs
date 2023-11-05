using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Models;

namespace server.Web.Controllers;

[ApiController, Route("api/customer/balance"), Authorize]
public class CustomerBalanceController : ControllerBase
{
  private ICustomersService _customerService;
  private IBalanceService _balanceService;
  public CustomerBalanceController(ICustomersService customerService, 
    IBalanceService balanceService)
  {
    _customerService = customerService;
    _balanceService = balanceService;
  }

  // @desc Increasing the balance
  // @route PUT api/customer/balance
  [HttpPut]
  public async Task<IActionResult> Increase([FromBody] int money)
  {
    if (money <= 0) return BadRequest(new { Message = "Некорректные данные" });

    int customerId = int.Parse(User.Identity.Name);
    Customer? customer = await _customerService.FindCustomer(c => c.Id == customerId);

    if (customer == null) return NotFound(new { Message = "Пользователь не найден" });

    await _balanceService.IncreaseBalance(customer, money);

    return Ok(new 
    { 
      Message = $"{customer.FirstName}, Вы успешно пополнили счёт на {money} рублей!" 
    });
  }
}