using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace server.Infrastructure.Services;
public class ProductVideosService : IProductVideosService
{
  private ApplicationContext _db;
  public ProductVideosService(ApplicationContext db) =>
    _db = db;

  public async IAsyncEnumerable<ProductVideoDto> AddRangeVideos(IFormFileCollection videos, int productId)
  {
    foreach (var video in videos)
      yield return await AddVideo(video, productId);
  }

  public async Task<ProductVideoDto> AddVideo(IFormFile video, int productId)
  {
    ProductVideo? productVideo = null;

    productVideo = _db.ProductVideos
    .Add(new ProductVideo()
    {
      ProductId = productId
    }).Entity;
    await _db.SaveChangesAsync();

    string extensionVideo = video.FileName.Substring(video.FileName.LastIndexOf('.'));
    string fileName = $"{productVideo.Id}{extensionVideo}";

    productVideo.Path = $"https://localhost:8888/upload/product-videos/{fileName}";
    await _db.SaveChangesAsync();

    using (FileStream fs = new FileStream($"./wwwroot/upload/product-videos/{fileName}", FileMode.Create))
      await video.CopyToAsync(fs);

    return new ProductVideoDto()
    {
      Id = productVideo.Id,
      Path = productVideo.Path,
      ProductId = productVideo.ProductId
    };
  }

  public async Task DeleteVideo(ProductVideo deletedVideo)
  {
    string? fileName = deletedVideo.Path?.Substring(deletedVideo.Path.LastIndexOf('/') + 1);

    File.Delete($"./wwwroot/upload/product-videos/{fileName}");

    _db.ProductVideos.Remove(deletedVideo);
    await _db.SaveChangesAsync();
  }

  public async Task<ProductVideo?> FindVideo(Expression<Func<ProductVideo, bool>> predicate) =>
    await _db.ProductVideos.FirstOrDefaultAsync(predicate);
}