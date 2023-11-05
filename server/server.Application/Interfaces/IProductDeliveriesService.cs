using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IProductDeliveriesService
{
  public Task<Delivery?> FindDelivery(Expression<Func<Delivery, bool>> predicate);
  public Task<Delivery> AddDelivery(AddedDeliveryDto addedDelivery, Product product);
  public Task DeleteDelivery(Delivery deletedDelivery, Product product);
  public Task ChangeDelivery(ChangedDeliveryDto changedDelivery, 
    Delivery initialDelivery, Product product);
  public Task ChangeDelivery(ChangedDeliveryDto changedDelivery,
    Delivery initialDelivery, Product deliveredProduct, Product initialProduct);
  public int GetCountDeliveries();
  public IEnumerable<DeliveryDto> GetAllDeliveries();
  public IEnumerable<DeliveryDto> GetRangeOfDeliveries(int limit, int page);
}
