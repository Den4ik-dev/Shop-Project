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
public interface IProductVideosService
{
  public Task<ProductVideoDto> AddVideo(IFormFile video, int productId);
  public IAsyncEnumerable<ProductVideoDto> AddRangeVideos(IFormFileCollection videos, int productId);
  public Task<ProductVideo?> FindVideo(Expression<Func<ProductVideo, bool>> predicate);
  public Task DeleteVideo(ProductVideo deletedVideo);
}
