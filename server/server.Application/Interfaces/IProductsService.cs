using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IProductsService
{
  public Task<Product?> FindProduct(Expression<Func<Product, bool>> predicate);
  public Task<Product> AddProduct(AddedProductDto addedProduct);
  public Task DeleteProduct(Product deletedProduct);
  public Task ChangeProduct(Product initialProduct, ChangedProductDto changedProduct);
  public IEnumerable<ProductDto> GetAllProducts();
  public int GetCountProducts();
  public IEnumerable<ProductDto> GetRangeOfProducts(int limit, int page);
  public Task<ProductDto> GetAllProductInfo(Expression<Func<Product, bool>> predicate);
  public IEnumerable<ProductDto> GetRangeOfProductsByCategoryId(int limit, int page, int categoryId);
  public IEnumerable<ProductDto> GetRangeOfProductsByText(int limit, int page, string text);
  public int GetCountProducts(Expression<Func<Product, bool>> predicate);
}