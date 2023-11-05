using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;
using server.Infrastructure.Services;

namespace server.Web.Controllers;
[ApiController, Route("api/products/manufacturers")]
public class ProductManufacturersController : ControllerBase
{
  // Add Put Delete Get
  private IProductManufacturersService _productManufacturersService;
  public ProductManufacturersController(IProductManufacturersService productManufacturersService)
  {
    _productManufacturersService = productManufacturersService;
  }

  // @desc Add new manufacturer
  // @route POST api/products/manufacturers
  [HttpPost, Authorize(Roles = "admin")]
  public async Task<IActionResult> Add([FromBody] string manufacturerName)
  {
    if (string.IsNullOrEmpty(manufacturerName))
      return BadRequest(new { Message = "Некорректные данные" });

    if (await _productManufacturersService.FindManufacturer(manufacturerName) != null)
      return BadRequest(new { Message = "Производитель с таким названием уже существует" });

    Manufacturer manufacturer = 
      await _productManufacturersService.AddManufacturer(manufacturerName);

    return Ok(new ManufacturerDto()
    {
      Id = manufacturer.Id,
      ManufacturerName = manufacturer.ManufacturerName
    });
  }

  // @desc Delete manufacturer
  // @route DELETE api/products/manufacturers/{id:int}
  [HttpDelete("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Delete(int id)
  {
    Manufacturer? manufacturer = await _productManufacturersService.FindManufacturer(id);

    if (manufacturer == null) return NotFound(new { Message = "Производитель не найдена" });

    await _productManufacturersService.DeleteManufacturer(manufacturer);

    return Ok(new { Message = $"Производитель '{manufacturer.ManufacturerName}' успешно удален" });
  }

  // @desc Change manufacturer
  // @route PUT api/products/manufacturers/{id:int}
  [HttpPut("{id:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Change(int id, [FromBody] string manufacturerName)
  {
    if (string.IsNullOrEmpty(manufacturerName))
      return BadRequest(new { Message = "Некорректные данные" });

    Manufacturer? manufacturer = await _productManufacturersService.FindManufacturer(id);

    if (manufacturer == null) return NotFound("Производитель не найден");

    await _productManufacturersService.ChangeManufacturer(manufacturer, manufacturerName);

    return Ok(new { Message = "Производитель успешно изменен" });
  }

  // @desc Get all manufacturers
  // @route GET api/products/manufacturers/all
  [HttpGet, Route("all")]
  public IActionResult GetAll() =>
    Ok(_productManufacturersService.GetAllManufacturers());

  // @desc Get range of manufacturers
  // @route GET api/products/manufacturers/range
  [HttpGet("range")]
  public IActionResult GetRange([FromQuery] int limit, [FromQuery] int page)
  {
    Response.Headers.Add(
      "x-total-count", 
      _productManufacturersService.GetCountManufacturers().ToString());

    return Ok(_productManufacturersService.GetRangeOfManufacturers(limit, page));
  }

  // @desc Get range of manufacturers
  // @route GET api/products/manufacturers/{id:int}
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetProductManufacturer(int id) =>
    Ok((await _productManufacturersService.FindManufacturer(m => m.Id == id))?.ManufacturerName);
}