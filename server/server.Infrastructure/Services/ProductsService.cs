using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class ProductsService : IProductsService
{
  private ApplicationContext _db;
  public ProductsService(ApplicationContext db) =>
    _db = db;

  public async Task<Product?> FindProduct(Expression<Func<Product, bool>> predicate) =>
    await _db.Products.FirstOrDefaultAsync(predicate);

  public async Task<ProductDto> GetAllProductInfo(Expression<Func<Product, bool>> predicate)
  {
    Product? product = await _db.Products
      .Include(p => p.Images)
      .Include(p => p.Videos)
      .FirstOrDefaultAsync(predicate);

    if (product is null) return null;

    return new ProductDto()
    {
      Id = product.Id,
      ProductName = product.ProductName,
      ProductDescription = product.ProductDescription,
      UnitPrice = product.UnitPrice,
      QuantityInStoke = product.QuantityInStoke,
      CategoryId = product.CategoryId,
      ManufacturerId = product.ManufacturerId,
      ImagesIds = product.Images.Select(img => img.Id),
      VideosIds = product.Videos.Select(video => video.Id),
    };
  }
  public async Task<Product> AddProduct(AddedProductDto addedProduct)
  {
    Product product = _db.Products.Add(new Product()
    {
      ProductName = addedProduct.ProductName,
      ProductDescription = addedProduct.ProductDescription,
      UnitPrice = addedProduct.UnitPrice,
      QuantityInStoke = addedProduct.QuantityInStoke,
      CategoryId = addedProduct.CategoryId,
      ManufacturerId = addedProduct.ManufacturerId
    }).Entity;

    await _db.SaveChangesAsync();

    return product;
  }

  public async Task DeleteProduct(Product deletedProduct)
  {
    _db.Products.Remove(deletedProduct);

    await _db.SaveChangesAsync();
  }

  public async Task ChangeProduct(Product initialProduct,
    ChangedProductDto changedProduct)
  {
    initialProduct.ProductName = changedProduct.ProductName;
    initialProduct.ProductDescription = changedProduct.ProductDescription;
    initialProduct.UnitPrice = changedProduct.UnitPrice;
    initialProduct.QuantityInStoke = changedProduct.QuantityInStoke;
    initialProduct.CategoryId = changedProduct.CategoryId;
    initialProduct.ManufacturerId = changedProduct.ManufacturerId;

    await _db.SaveChangesAsync();
  }

  public IEnumerable<ProductDto> GetAllProducts() =>
    _db.Products
      .Include(p => p.Images)
      .Include(p => p.Videos)
      .Select(p => new ProductDto()
      {
        Id = p.Id,
        ProductName = p.ProductName,
        ProductDescription = p.ProductDescription,
        UnitPrice = p.UnitPrice,
        QuantityInStoke = p.QuantityInStoke,
        CategoryId = p.CategoryId,
        ManufacturerId = p.ManufacturerId,
        ImagesIds = p.Images.Select(img => img.Id),
        VideosIds = p.Videos.Select(video => video.Id),
      });

  public IEnumerable<ProductDto> GetRangeOfProducts(int limit, int page) =>
    _db.Products
      .Skip(limit * page)
      .Take(limit)
      .Include(p => p.Images)
      .Include(p => p.Videos)
      .Select(p => new ProductDto()
      {
        Id = p.Id,
        ProductName = p.ProductName,
        ProductDescription = p.ProductDescription,
        UnitPrice = p.UnitPrice,
        QuantityInStoke = p.QuantityInStoke,
        CategoryId = p.CategoryId,
        ManufacturerId = p.ManufacturerId,
        ImagesIds = p.Images.Select(img => img.Id),
        VideosIds = p.Videos.Select(video => video.Id),
      });

  public IEnumerable<ProductDto> GetRangeOfProductsByCategoryId(int limit, int page, int categoryId) =>
    _db.Products
      .Where(p => p.CategoryId == categoryId)
      .Skip(limit * page)
      .Take(limit)
      .Include(p => p.Images)
      .Include(p => p.Videos)
      .Select(p => new ProductDto()
      {
        Id = p.Id,
        ProductName = p.ProductName,
        ProductDescription = p.ProductDescription,
        UnitPrice = p.UnitPrice,
        QuantityInStoke = p.QuantityInStoke,
        CategoryId = p.CategoryId,
        ManufacturerId = p.ManufacturerId,
        ImagesIds = p.Images.Select(img => img.Id),
        VideosIds = p.Videos.Select(video => video.Id),
      });

  public IEnumerable<ProductDto> GetRangeOfProductsByText(int limit, int page, string text) =>
    _db.Products
      .Where(p => EF.Functions.Like(p.ProductName.ToLower(), $"%{text.ToLower()}%"))
      .Skip(limit * page)
      .Take(limit)
      .Include(p => p.Images)
      .Include(p => p.Videos)
      .Select(p => new ProductDto()
      {
        Id = p.Id,
        ProductName = p.ProductName,
        ProductDescription = p.ProductDescription,
        UnitPrice = p.UnitPrice,
        QuantityInStoke = p.QuantityInStoke,
        CategoryId = p.CategoryId,
        ManufacturerId = p.ManufacturerId,
        ImagesIds = p.Images.Select(img => img.Id),
        VideosIds = p.Videos.Select(video => video.Id),
      });

  public int GetCountProducts() => _db.Products.Count();
  public int GetCountProducts(Expression<Func<Product, bool>> predicate) => _db.Products.Count(predicate);
}