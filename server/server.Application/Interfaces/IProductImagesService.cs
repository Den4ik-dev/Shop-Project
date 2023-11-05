using Microsoft.AspNetCore.Http;
using server.Domain.Dto;
using server.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace server.Application.Interfaces;
public interface IProductImagesService
{
  public Task<ProductImageDto> AddImage(IFormFile image, int productId);
  public IAsyncEnumerable<ProductImageDto> AddRangeImages(IFormFileCollection images, int productId);
  public Task<ProductImage?> FindImage(Expression<Func<ProductImage, bool>> predicate);
  public Task DeleteImage(ProductImage deletedImage);
}