using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface ICustomerBasketsService
{
  public int GetCountBasketItems(int customerId);
  public IEnumerable<BasketItemDto> GetRangeOfBasketItems(int customerId, int limit, int page);
  public IEnumerable<BasketItemDto> GetAllBasketItems(int customerId);
  public Task<BasketItem> AddBasketItem(int customerId, AddedBasketItemDto addedBasketItem);
  public Task<BasketItem?> FindBasketItem(Expression<Func<BasketItem, bool>> predicate);
  public Task DeleteBasketItem(BasketItem basketItem);
  public Task ChangeBasketItem(BasketItem basketItem, int productCount);
}