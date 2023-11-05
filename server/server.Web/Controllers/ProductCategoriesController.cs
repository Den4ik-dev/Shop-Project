using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;
using server.Infrastructure.Services;

namespace server.Web.Controllers;

[ApiController, Route("api/products/categories")]
public class ProductCategoriesController : ControllerBase
{
  private IProductCategoriesService _productCategoriesService;
  public ProductCategoriesController(IProductCategoriesService productCategoriesService) =>
    _productCategoriesService = productCategoriesService;


  // @desc Add new category
  // @route POST api/products/categories
  [HttpPost, Authorize(Roles = "admin")]
  public async Task<IActionResult> Add([FromBody] string categoryName)
  {
    if (string.IsNullOrEmpty(categoryName))
      return BadRequest(new { Message = "Некорректные данные" });

    if (await _productCategoriesService.FindCategory(categoryName) != null)
      return BadRequest(new { Message = "Категория с таким названием уже существует" });

    Category category = await _productCategoriesService.AddCategory(categoryName);

    return Ok(new CategoryDto()
    {
      Id = category.Id,
      CategoryName = category.CategoryName
    });
  }

  // @desc Delete category
  // @route DELETE api/products/categories/{id:int}
  [HttpDelete("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Delete(int id)
  {
    Category? category = await _productCategoriesService.FindCategory(id);

    if (category == null) return NotFound(new { Message = "Категория не найдена" });

    await _productCategoriesService.DeleteCategory(category);

    return Ok(new { Message = $"Категория '{category.CategoryName}' успешно удалена" });
  }

  // @desc Change category
  // @route PUT api/products/categories/{id:int}
  [HttpPut("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Change(int id, [FromBody] string categoryName)
  {
    if (string.IsNullOrEmpty(categoryName))
      return BadRequest(new { Message = "Некорректные данные" });

    Category? category = await _productCategoriesService.FindCategory(id);

    if (category == null) return NotFound("Категория не найдена");

    await _productCategoriesService.ChangeCategory(category, categoryName);

    return Ok(new { Message = "Категория успешно изменена" });
  }

  // @desc Get all categories
  // @route GET api/products/categories/all
  [HttpGet, Route("all")]
  public IActionResult GetAll() =>
    Ok(_productCategoriesService.GetAllCategories());

  // @desc Get range of categories
  // @route GET api/products/categories/range
  [HttpGet("range")]
  public IActionResult GetRange([FromQuery] int limit, [FromQuery] int page)
  {
    Response.Headers.Add(
      "x-total-count",
      _productCategoriesService.GetCountCategories().ToString());

    return Ok(_productCategoriesService.GetRangeOfCategories(limit, page));
  }

  // @desc Get category
  // @route GET api/products/categories/{id:int}
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetProductCategory(int id) =>
    Ok((await _productCategoriesService.FindCategory(c => c.Id == id))?.CategoryName);
}