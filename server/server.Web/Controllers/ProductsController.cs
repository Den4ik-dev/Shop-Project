using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;

namespace server.Web.Controllers;
[ApiController, Route("api/products")]
public class ProductsController : ControllerBase
{
  private IProductsService _productsService;
  private IProductCategoriesService _productCategoriesService;
  private IProductManufacturersService _productManufacturersService;
  public ProductsController(IProductsService productsService, 
    IProductCategoriesService productCategoriesService,
    IProductManufacturersService productManufacturersService)
  {
    _productsService = productsService;
    _productCategoriesService = productCategoriesService;
    _productManufacturersService = productManufacturersService;
  }

  [HttpPost, Authorize(Roles = "admin")]
  public async Task<IActionResult> Add([FromBody] AddedProductDto addedProduct,
    [FromServices] IValidator<AddedProductDto> addedProductValidator)
  {
    var addedProductValidatorResult = addedProductValidator.Validate(addedProduct);
    if (!addedProductValidatorResult.IsValid)
      return BadRequest(new { Message = addedProductValidatorResult.Errors.First().ErrorMessage });

    if (await _productsService.FindProduct(p => p.ProductName == addedProduct.ProductName) != null)
      return BadRequest(new { Message = "Продукт с данным названием уже существует" });

    if(await _productCategoriesService.FindCategory(c => c.Id == addedProduct.CategoryId) == null)
      return BadRequest(new { Message = "Категория не найден" });

    if(await _productManufacturersService
      .FindManufacturer(m => m.Id == addedProduct.ManufacturerId) == null)
      return BadRequest(new { Message = "Производитель не найден" });

    Product product = await _productsService.AddProduct(addedProduct);

    return Ok(new { Message = "Продукт успешно добавлен" });
  }

  [HttpDelete("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Delete(int id)
  {
    Product? product = await _productsService.FindProduct(p => p.Id == id);

    if (product == null)
      return NotFound(new { Message = "Продукт не найден" });

    await _productsService.DeleteProduct(product);

    return Ok(new { Message = "Продукт успешно удалён" });
  }

  [HttpPut("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Change(int id,
    [FromBody] ChangedProductDto changedProduct,
    [FromServices] IValidator<ChangedProductDto> changedProductValidator)
  {
    var changedProductValidatorResult = changedProductValidator.Validate(changedProduct);
    if (!changedProductValidatorResult.IsValid)
      return BadRequest(new { Message = changedProductValidatorResult.Errors.First().ErrorMessage });

    Product? product = await _productsService.FindProduct(p => p.Id == id);

    if (product == null)
      return NotFound(new { Message = "Продукт не найден" });

    if (await _productCategoriesService.FindCategory(c => c.Id == changedProduct.CategoryId) == null)
      return BadRequest(new { Message = "Категория не найден" });

    if (await _productManufacturersService
      .FindManufacturer(m => m.Id == changedProduct.ManufacturerId) == null)
      return BadRequest(new { Message = "Производитель не найден" });

    await _productsService.ChangeProduct(product, changedProduct);

    return Ok(new { Message = "Продукт успешно изменён" });
  }

  [HttpGet, Route("all")]
  public IActionResult GetAll() =>
    Ok(_productsService.GetAllProducts());

  [HttpGet]
  public IActionResult GetRange(int limit, int page) /* int categoryId, string text => description, productName */
  {
    Response.Headers.Add(
      "x-total-count",
      _productsService.GetCountProducts().ToString());

    return Ok(_productsService.GetRangeOfProducts(limit, page));
  }
  [HttpGet("categories")]
  public IActionResult GetRangeByCategoryId(int limit, int page, int categoryId)
  {
    Response.Headers.Add(
      "x-total-count",
      _productsService.GetCountProducts(p => p.CategoryId == categoryId).ToString());

    return Ok(_productsService.GetRangeOfProductsByCategoryId(limit, page, categoryId));
  }
  [HttpGet("input")]
  public IActionResult GetRangeByText(int limit, int page, string text)
  {
    Response.Headers.Add(
      "x-total-count",
      _productsService.GetCountProducts(p => EF.Functions.Like(p.ProductName.ToLower(), $"%{text.ToLower()}%")).ToString());

    return Ok(_productsService.GetRangeOfProductsByText(limit, page, text));
  }


  [HttpGet("{id:int}")]
  public async Task<ProductDto?> GetProduct(int id) =>
    await _productsService.GetAllProductInfo(p => p.Id == id);
}