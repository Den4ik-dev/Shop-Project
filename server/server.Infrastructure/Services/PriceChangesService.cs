using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class PriceChangesService : IPriceChangesService
{
  private ApplicationContext _db;

  public PriceChangesService(ApplicationContext db) =>
    _db = db;

  public async Task<PriceChange?> FindPriceChange(Expression<Func<PriceChange, bool>> predicate) =>
    await _db.PriceChanges.FirstOrDefaultAsync(predicate);

  public async Task<PriceChange> AddPriceChange(AddedPriceChangeDto addedPriceChange)
  {
    PriceChange priceChange = _db.PriceChanges.Add(new PriceChange()
    {
      NewPrice = addedPriceChange.NewPrice,
      ProductId = addedPriceChange.ProductId
    }).Entity;

    await _db.SaveChangesAsync();

    return priceChange;
  }

  public async Task DeletePriceChange(PriceChange deletedPriceChange)
  {
    _db.PriceChanges.Remove(deletedPriceChange);

    await _db.SaveChangesAsync();
  }

  public async Task ChangePriceChange(PriceChange initialPriceChange, 
    ChangedPriceChangeDto changedPriceChange)
  {
    initialPriceChange.NewPrice = changedPriceChange.NewPrice;
    initialPriceChange.ProductId = changedPriceChange.ProductId;

    await _db.SaveChangesAsync();
  }

  public IEnumerable<PriceChangeDto> GetAllPriceChanges(int productId) =>
    _db.PriceChanges
      .Where(pch => pch.ProductId == productId)
      .Select(pch => new PriceChangeDto()
      {
        Id = pch.Id,
        NewPrice = pch.NewPrice,
        DatePriceChange = pch.DatePriceChange,
        ProductId = pch.ProductId
      });

  public IEnumerable<PriceChangeDto> GetRangeOfPriceChanges(int productId, int limit, int page) =>
    _db.PriceChanges
      .Where(pch => pch.ProductId == productId)
      .Skip(limit * page)
      .Take(limit)
      .Select(pch => new PriceChangeDto()
      {
        Id = pch.Id,
        NewPrice = pch.NewPrice,
        DatePriceChange = pch.DatePriceChange,
        ProductId = pch.ProductId
      });

  public int GetCountPriceChanges(int productId) => 
    _db.PriceChanges.Count(pch => pch.ProductId == productId);

  public async Task<PriceChange?> GetLastPriceChange(int productId) =>
    await _db.PriceChanges
      .OrderBy(pch => pch.Id)
      .LastOrDefaultAsync(pch => pch.ProductId == productId);
}