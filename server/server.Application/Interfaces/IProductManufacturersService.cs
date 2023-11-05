

using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface IProductManufacturersService
{
  public Task<Manufacturer?> FindManufacturer(string manufacturerName);
  public Task<Manufacturer?> FindManufacturer(int manufacturerId);
  public Task<Manufacturer?> FindManufacturer(Expression<Func<Manufacturer, bool>> predicate);
  public Task<Manufacturer> AddManufacturer(string manufacturerName);
  public Task DeleteManufacturer(Manufacturer manufacturer);
  public Task ChangeManufacturer(Manufacturer manufacturer, string changedManufacturerName);
  public IEnumerable<ManufacturerDto> GetAllManufacturers();
  public IEnumerable<ManufacturerDto> GetRangeOfManufacturers(int limit, int page);
  public int GetCountManufacturers();
}