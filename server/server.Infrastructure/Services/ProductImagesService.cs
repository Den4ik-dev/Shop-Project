using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace server.Infrastructure.Services;
public class ProductImagesService : IProductImagesService
{
  private ApplicationContext _db;
  public ProductImagesService(ApplicationContext db) =>
    _db = db;
  public async Task<ProductImageDto> AddImage(IFormFile image, int productId)
  {
    ProductImage? productImage = null;

    productImage = _db.ProductImages
    .Add(new ProductImage()
    {
      ProductId = productId
    }).Entity;
    await _db.SaveChangesAsync();

    string extensionImage = image.FileName.Substring(image.FileName.LastIndexOf('.'));
    string fileName = $"{productImage.Id}{extensionImage}";

    productImage.Path = $"https://localhost:8888/upload/product-images/{fileName}";
    await _db.SaveChangesAsync();

    using (FileStream fs = new FileStream($"./wwwroot/upload/product-images/{fileName}", FileMode.Create))
      await image.CopyToAsync(fs);
    
    return new ProductImageDto() 
    { 
      Id = productImage.Id,
      Path = productImage.Path,
      ProductId = productImage.ProductId
    };
  }

  public async IAsyncEnumerable<ProductImageDto> AddRangeImages(IFormFileCollection images, int productId)
  {
    foreach (var image in images)
      yield return await AddImage(image, productId);
  }

  public async Task<ProductImage?> FindImage(Expression<Func<ProductImage, bool>> predicate) =>
    await _db.ProductImages.FirstOrDefaultAsync(predicate);

  public async Task DeleteImage(ProductImage deletedImage)
  {
    string? fileName = deletedImage.Path?.Substring(deletedImage.Path.LastIndexOf('/') + 1);

    File.Delete($"./wwwroot/upload/product-images/{fileName}");

    _db.ProductImages.Remove(deletedImage);
    await _db.SaveChangesAsync();
  }
}