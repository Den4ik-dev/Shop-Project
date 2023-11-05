using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;

namespace server.Web.Controllers;
[ApiController, Route("api/products/deliveries"), Authorize(Roles = "admin")]
public class ProductDeliveriesController : ControllerBase
{
  private IProductsService _productsService;
  private IProductDeliveriesService _productDeliveriesService;
  public ProductDeliveriesController(IProductsService productsService,
    IProductDeliveriesService productDeliveriesService)
  {
    _productsService = productsService;
    _productDeliveriesService = productDeliveriesService;
  }

  [HttpPost]
  public async Task<IActionResult> Add([FromBody] AddedDeliveryDto addedDelivery,
    [FromServices] IValidator<AddedDeliveryDto> addedDeliveryValidator)
  {
    var addedDeliveryValidatorResult = addedDeliveryValidator.Validate(addedDelivery);
    if (!addedDeliveryValidatorResult.IsValid)
      return BadRequest(new { Message = addedDeliveryValidatorResult.Errors.First().ErrorMessage });

    Product? product = await _productsService.FindProduct(p => p.Id == addedDelivery.ProductId);

    if (product == null)
      return BadRequest(new { Message = "Данного продукта не существует" });

    Delivery delivery = await _productDeliveriesService.AddDelivery(addedDelivery, product);

    return Ok(new DeliveryDto()
    {
      Id = delivery.Id,
      ProductCount = delivery.ProductCount,
      ProductId = delivery.ProductId,
      DeliveryDate = delivery.DeliveryDate
    });
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> Delete(int id)
  {
    Delivery? deletedDelivery = await _productDeliveriesService.FindDelivery(d => d.Id == id);

    if (deletedDelivery == null)
      return NotFound(new { Message = "Поставка не найдена" });

    Product? product = await _productsService.FindProduct(p => p.Id == deletedDelivery.ProductId);

    await _productDeliveriesService.DeleteDelivery(deletedDelivery, product);

    return Ok(new { Message = "Поставка успешно удалена" });
  }

  [HttpPut("{id:int}")]
  public async Task<IActionResult> Change(int id,
    [FromBody] ChangedDeliveryDto changedDelivery,
    [FromServices] IValidator<ChangedDeliveryDto> changedDeliveryValidator)
  {
    var changedDeliveryValidatorResult = changedDeliveryValidator.Validate(changedDelivery);
    if (!changedDeliveryValidatorResult.IsValid)
      return BadRequest(new { Message = changedDeliveryValidatorResult.Errors.First().ErrorMessage });

    Delivery? delivery = await _productDeliveriesService.FindDelivery(d => d.Id == id);

    if (delivery == null)
      return NotFound(new { Message = "Поставка не найдена" });

    if (delivery.ProductId == changedDelivery.ProductId)
    {
      Product? product = await _productsService.FindProduct(p => p.Id == delivery.ProductId);

      await _productDeliveriesService.ChangeDelivery(changedDelivery, delivery, product);
    }
    else
    {
      Product? deliveredProduct = await _productsService.FindProduct(p => p.Id == changedDelivery.ProductId);

      if (deliveredProduct == null)
        return BadRequest(new { Message = "Данного продукта не существует" });

      Product? initialProduct = await _productsService.FindProduct(p => p.Id == delivery.ProductId);

      await _productDeliveriesService.ChangeDelivery(changedDelivery, delivery,
        deliveredProduct, initialProduct);
    }

    return Ok(new { Message = "Поставка успешно изменена" });
  }

  [HttpGet, Route("all")]
  public IActionResult GetAll() =>
    Ok(_productDeliveriesService.GetAllDeliveries());

  [HttpGet]
  public IActionResult GetRange([FromQuery] int limit, [FromQuery] int page)
  {
    Response.Headers.Add(
      "x-total-count",
      _productDeliveriesService.GetCountDeliveries().ToString());

    return Ok(_productDeliveriesService.GetRangeOfDeliveries(limit, page));
  }
}
