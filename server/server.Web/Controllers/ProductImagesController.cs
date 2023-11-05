using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;

namespace server.Web.Controllers;

[ApiController, Route("api/products/images")]
public class ProductImagesController : ControllerBase
{
  private IProductImagesService _productImagesService;
  public ProductImagesController(IProductImagesService productImagesService) =>
    _productImagesService = productImagesService;


  [HttpPost("{productId:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Add(int productId, 
    [FromServices] IProductsService productsService)
  {
    if (await productsService.FindProduct(p => p.Id == productId) == null)
      return NotFound(new { Message = "Продукта с данным идентификатором не существует" });

    IAsyncEnumerable<ProductImageDto> images = _productImagesService.AddRangeImages(
      images: Request.Form.Files, productId);

    return Ok(images);
  }

  [HttpDelete("{imageId:int}"), Authorize(Roles = "admin")]
  public async Task<IActionResult> Delete(int imageId)
  {
    ProductImage? image = await _productImagesService.FindImage(image => image.Id == imageId);

    if (image == null) return NotFound(new { Message = "Изобращение не найдено" });

    await _productImagesService.DeleteImage(image);

    return Ok(new { Message = "Изобращение успешно удалено" });
  }

  [HttpGet("{imageId:int}")]
  public async Task<IActionResult> Get(int imageId)
  {
    ProductImage? image = await _productImagesService.FindImage(image => image.Id == imageId);

    if (image == null) return NotFound(new { Message = "Изображение не найдено" });

    return Ok(image.Path);
  }
}