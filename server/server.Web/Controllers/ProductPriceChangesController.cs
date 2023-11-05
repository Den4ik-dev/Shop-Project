using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;

namespace server.Web.Controllers;
[ApiController, Route("api/products/price-changes")]
public class ProductPriceChangesController : ControllerBase
{
  private IProductsService _productsService;
  private IPriceChangesService _priceChangesService;
  public ProductPriceChangesController(IProductsService productsService,
    IPriceChangesService priceChangesService)
  {
    _productsService = productsService;
    _priceChangesService = priceChangesService;
  }

  [HttpPost, Authorize(Roles = "admin")]
  public async Task<IActionResult> Add([FromBody] AddedPriceChangeDto addedPriceChange,
    [FromServices] IValidator<AddedPriceChangeDto> addedPriceChangeValidator)
  {
    var addedPriceChangeValidatorResult = addedPriceChangeValidator.Validate(addedPriceChange);
    if (!addedPriceChangeValidatorResult.IsValid)
      return BadRequest(new { Message = addedPriceChangeValidatorResult.Errors.First().ErrorMessage });

    Product? product = await _productsService.FindProduct(p => p.Id == addedPriceChange.ProductId);

    if (product == null)
      return BadRequest(new { Message = "Данный продукт не найден" });

    PriceChange priceChange = await _priceChangesService.AddPriceChange(addedPriceChange);

    return Ok(new PriceChangeDto()
    {
      Id = priceChange.Id,
      ProductId = priceChange.ProductId,
      NewPrice = priceChange.NewPrice,
      DatePriceChange = priceChange.DatePriceChange
    });
  }

  [HttpDelete("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Delete(int id)
  {
    PriceChange? priceChange = await _priceChangesService.FindPriceChange(pch => pch.Id == id);

    if (priceChange == null)
      return NotFound(new { Message = "Изменение цены с данным идентификатором не найдено" });

    await _priceChangesService.DeletePriceChange(priceChange);

    return Ok(new { Message = "Изменение цены успешно удалено" });
  }

  [HttpPut("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Change(int id, [FromBody] ChangedPriceChangeDto changedPriceChange,
    [FromServices] IValidator<ChangedPriceChangeDto> changedPriceChangeValidator)
  {
    var changedPriceChangeValidatorResult = changedPriceChangeValidator.Validate(changedPriceChange);
    if (!changedPriceChangeValidatorResult.IsValid)
      return BadRequest(new { Message = changedPriceChangeValidatorResult.Errors.First().ErrorMessage });

    PriceChange? priceChange = await _priceChangesService.FindPriceChange(pch => pch.Id == id);

    if (priceChange == null)
      return NotFound(new { Message = "Изменение цены не найдено" });

    Product? product = await _productsService.FindProduct(p => p.Id == changedPriceChange.ProductId);

    if (product == null)
      return BadRequest(new { Message = "Продукт с данным идентификатором не найден" });

    await _priceChangesService.ChangePriceChange(priceChange, changedPriceChange);

    return Ok(new { Message = "Изменение цены успешно изменено" });
  }

  [HttpGet, Authorize(Roles = "admin"), Route("all")]
  public IActionResult GetAll(int productId) =>
    Ok(_priceChangesService.GetAllPriceChanges(productId));

  [HttpGet, Authorize(Roles = "admin"), Route("range")]
  public IActionResult GetRange([FromQuery] int productId, [FromQuery] int limit, [FromQuery] int page)
  {
    Response.Headers.Add(
      "x-total-count",
      _priceChangesService.GetCountPriceChanges(productId).ToString()
    );

    return Ok(_priceChangesService.GetRangeOfPriceChanges(productId, limit, page));
  }

  [HttpGet]
  public async Task<IActionResult> GetLast([FromQuery] int productId)
  {
    PriceChange? priceChange = await _priceChangesService.GetLastPriceChange(productId);

    if (priceChange == null)
      return NotFound(new { Message = "Изменение цены не найдено" });

    return Ok(new PriceChangeDto()
    {
      Id = priceChange.Id,
      NewPrice = priceChange.NewPrice,
      DatePriceChange = priceChange.DatePriceChange,
      ProductId = priceChange.ProductId
    });
  }
}