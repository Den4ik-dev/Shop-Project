using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IPriceChangesService
{
  public Task<PriceChange?> FindPriceChange(Expression<Func<PriceChange, bool>> predicate);
  public Task<PriceChange> AddPriceChange(AddedPriceChangeDto addedPriceChange);
  public Task DeletePriceChange(PriceChange deletedPriceChange);
  public Task ChangePriceChange(PriceChange initialPriceChange,
    ChangedPriceChangeDto changedPriceChange);
  public IEnumerable<PriceChangeDto> GetAllPriceChanges(int productId);
  public IEnumerable<PriceChangeDto> GetRangeOfPriceChanges(int productId, int limit, int page);
  public int GetCountPriceChanges(int productId);
  public Task<PriceChange?> GetLastPriceChange(int productId);
}