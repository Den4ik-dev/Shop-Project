using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;

namespace server.Web.Controllers;
[ApiController, Route("api/customers/baskets"), Authorize]
public class CustomerBasketController : ControllerBase
{
  private ICustomerBasketsService _customerBasketsService;
  private IProductsService _productsService;
  public CustomerBasketController(ICustomerBasketsService customerBasketsService, 
    IProductsService productsService)
  {
    _customerBasketsService = customerBasketsService;
    _productsService = productsService;
  }

  [HttpGet]
  public IActionResult GetRange([FromQuery] int limit, [FromQuery] int page)
  {
    int userId = int.Parse(User.Identity.Name);

    Response.Headers.Add(
      "x-total-count",
      _customerBasketsService.GetCountBasketItems(userId).ToString());

    return Ok(_customerBasketsService.GetRangeOfBasketItems(userId, limit, page));
  }

  [HttpGet("all")]
  public IActionResult GetAll()
  {
    int userId = int.Parse(User.Identity.Name);
    return Ok(_customerBasketsService.GetAllBasketItems(userId));
  }

  [HttpPost]
  public async Task<IActionResult> Add([FromBody] AddedBasketItemDto addedBasketItem,
    [FromServices] IValidator<AddedBasketItemDto> addedBasketItemValidator)
  {
    var addedBasketItemValidatorResult = addedBasketItemValidator.Validate(addedBasketItem);

    if (!addedBasketItemValidatorResult.IsValid)
      return BadRequest(new { Message = addedBasketItemValidatorResult.Errors.First().ErrorMessage });

    int userId = int.Parse(User.Identity.Name);
    var product = await _productsService.FindProduct(p => p.Id == addedBasketItem.ProductId);

    if (product == null)
      return BadRequest(new { Message = "Данного продукта не существует" });

    if (await _customerBasketsService.FindBasketItem(bi => 
          bi.ProductId == product.Id && bi.CustomerId == userId) != null)
      return BadRequest(new { Message = "Данный продукт уже в корзине" });

    if (product.QuantityInStoke < addedBasketItem.ProductCount)
      return BadRequest(new { Message = "Такого количества продукта нет на складе" });

    var basketItem = await _customerBasketsService.AddBasketItem(userId, addedBasketItem);

    return Ok(new BasketItemDto()
    {
      Id = basketItem.Id,
      CustomerId = userId,
      ProductId = basketItem.ProductId,
      ProductCount = basketItem.ProductCount,
      ProductUnitPrice = product.UnitPrice
    });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    int userId = int.Parse(User.Identity.Name);

    BasketItem? deletedBasketItem = 
      await _customerBasketsService.FindBasketItem(bi => 
        bi.Id == id && bi.CustomerId == userId);

    if (deletedBasketItem == null)
      return BadRequest(new { Message = "Продукт в корзине не найден" });

    await _customerBasketsService.DeleteBasketItem(deletedBasketItem);

    return Ok(new { Message = "Продукт успешно удалён из корзины" });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Change(int id, int productCount)
  {
    if (productCount <= 0) return BadRequest(new { Message = "Некорректные данные" });

    BasketItem? basketItem = await _customerBasketsService.FindBasketItem(bi => bi.Id == id);

    if (basketItem == null) return BadRequest(new { Message = "Продукт не найден в корзине" });

    await _customerBasketsService.ChangeBasketItem(basketItem, productCount);
    return Ok();
  }
}