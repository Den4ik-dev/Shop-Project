using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class CustomerBasketsService : ICustomerBasketsService
{
  private ApplicationContext _db;
  public CustomerBasketsService(ApplicationContext db) =>
    _db = db;

  public int GetCountBasketItems(int customerId) =>
    _db.BasketItems.Count(bi => bi.CustomerId == customerId);

  public IEnumerable<BasketItemDto> GetRangeOfBasketItems(int customerId, int limit, int page) =>
    _db.BasketItems
      .Where(bi => bi.CustomerId == customerId)
      .Skip(limit * page)
      .Take(limit)
      .Select(bi => new BasketItemDto()
      {
        Id = bi.Id,
        ProductCount = bi.ProductCount,
        ProductId = bi.ProductId,
        CustomerId = bi.CustomerId,
        ProductUnitPrice = _db.Products.First(p => p.Id == bi.ProductId).UnitPrice
      });

  public IEnumerable<BasketItemDto> GetAllBasketItems(int customerId) =>
    _db.BasketItems
      .Where(bi => bi.CustomerId == customerId)
      .Select(bi => new BasketItemDto()
      {
        Id = bi.Id,
        ProductCount = bi.ProductCount,
        ProductId = bi.ProductId,
        CustomerId = bi.CustomerId,
        ProductUnitPrice = _db.Products.First(p => p.Id == bi.ProductId).UnitPrice
      });

  public async Task<BasketItem> AddBasketItem(int customerId, AddedBasketItemDto addedBasketItem)
  {
    BasketItem basketItem = _db.BasketItems.Add(new BasketItem()
    {
      CustomerId = customerId,
      ProductCount = addedBasketItem.ProductCount,
      ProductId = addedBasketItem.ProductId
    }).Entity;

    await _db.SaveChangesAsync();

    return basketItem;
  }

  public async Task<BasketItem?> FindBasketItem(Expression<Func<BasketItem, bool>> predicate) =>
    await _db.BasketItems.FirstOrDefaultAsync(predicate);

  public async Task DeleteBasketItem(BasketItem basketItem)
  {
    _db.BasketItems.Remove(basketItem);

    await _db.SaveChangesAsync();
  }

  public async Task ChangeBasketItem(BasketItem basketItem, int productCount)
  {
    basketItem.ProductCount = productCount;

    await _db.SaveChangesAsync();
  }
}