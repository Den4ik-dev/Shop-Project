using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Application.Interfaces;
using server.Domain.Dto;
using server.Domain.Models;

namespace server.Web.Controllers;
[ApiController, Route("api/products/videos"), Authorize(Roles = "admin")]
public class ProductVideosController : ControllerBase
{
  private IProductVideosService _productVideosService;
  public ProductVideosController(IProductVideosService productVideosService) =>
    _productVideosService = productVideosService;

  [HttpPost("{productId:int}")]
  public async Task<IActionResult> Add(int productId,
    [FromServices] IProductsService productsService)
  {
    if (await productsService.FindProduct(p => p.Id == productId) == null)
      return NotFound(new { Message = "Продукта с данным идентификатором не существует" });

    IAsyncEnumerable<ProductVideoDto> videos = _productVideosService.AddRangeVideos(
      videos: Request.Form.Files, productId);

    return Ok(videos);
  }

  [HttpDelete("{videoId:int}")]
  public async Task<IActionResult> Delete(int videoId)
  {
    ProductVideo? video = await _productVideosService.FindVideo(video => video.Id == videoId);

    if (video == null) return NotFound(new { Message = "Видео не найдено" });

    await _productVideosService.DeleteVideo(video);

    return Ok(new { Message = "Видео успешно удалено" });
  }
}
