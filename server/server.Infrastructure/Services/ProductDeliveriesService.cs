using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class ProductDeliveriesService : IProductDeliveriesService
{
  private IProductsService _productsService;
  private ApplicationContext _db;
  public ProductDeliveriesService(IProductsService productsService,
    ApplicationContext db)
  {
    _productsService = productsService;
    _db = db;
  }

  public async Task<Delivery?> FindDelivery(Expression<Func<Delivery, bool>> predicate) =>
    await _db.Deliveries.FirstOrDefaultAsync(predicate);

  public async Task<Delivery> AddDelivery(AddedDeliveryDto addedDelivery, Product product)
  {
    product.QuantityInStoke += addedDelivery.ProductCount;

    Delivery delivery = _db.Deliveries.Add(new Delivery()
    {
      ProductCount = addedDelivery.ProductCount,
      ProductId = addedDelivery.ProductId
    }).Entity;

    await _db.SaveChangesAsync();

    return delivery;
  }

  public async Task DeleteDelivery(Delivery deletedDelivery, Product product)
  {
    product.QuantityInStoke -= deletedDelivery.ProductCount;

    _db.Deliveries.Remove(deletedDelivery);

    await _db.SaveChangesAsync();
  }

  public async Task ChangeDelivery(ChangedDeliveryDto changedDelivery, 
    Delivery initialDelivery, Product product)
  {
    product.QuantityInStoke = 
      product.QuantityInStoke - initialDelivery.ProductCount + changedDelivery.ProductCount;

    initialDelivery.ProductCount = changedDelivery.ProductCount;

    await _db.SaveChangesAsync();
  }

  public async Task ChangeDelivery(ChangedDeliveryDto changedDelivery,
    Delivery initialDelivery, Product deliveredProduct, Product initialProduct)
  {
    initialProduct.QuantityInStoke -= initialDelivery.ProductCount;
    deliveredProduct.QuantityInStoke += changedDelivery.ProductCount;

    initialDelivery.ProductId = changedDelivery.ProductId;
    initialDelivery.ProductCount = changedDelivery.ProductCount;

    await _db.SaveChangesAsync();
  }
  public int GetCountDeliveries() => _db.Deliveries.Count();
  public IEnumerable<DeliveryDto> GetAllDeliveries() =>
    _db.Deliveries.Select(d => new DeliveryDto()
    {
      Id = d.Id,
      DeliveryDate = d.DeliveryDate,
      ProductCount = d.ProductCount,
      ProductId = d.ProductId
    });

  public IEnumerable<DeliveryDto> GetRangeOfDeliveries(int limit, int page) =>
    _db.Deliveries
      .Skip(page * limit)
      .Take(limit)
      .Select(d => new DeliveryDto()
      {
        Id = d.Id,
        DeliveryDate = d.DeliveryDate,
        ProductCount = d.ProductCount,
        ProductId = d.ProductId
      });
}