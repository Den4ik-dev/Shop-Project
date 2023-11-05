using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace server.Infrastructure.Services;
public class ProductManufacturersService : IProductManufacturersService
{
  private ApplicationContext _db;
  public ProductManufacturersService(ApplicationContext db) =>
    _db = db;

  public async Task<Manufacturer?> FindManufacturer(string manufacturerName) =>
    await _db.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerName == manufacturerName);

  public async Task<Manufacturer?> FindManufacturer(int manufacturerId) =>
    await _db.Manufacturers.FirstOrDefaultAsync(m => m.Id == manufacturerId);
  public async Task<Manufacturer?> FindManufacturer(Expression<Func<Manufacturer, bool>> predicate) =>
    await _db.Manufacturers.FirstOrDefaultAsync(predicate);

  public async Task<Manufacturer> AddManufacturer(string manufacturerName)
  {
    Manufacturer newManufacturer =
      _db.Manufacturers.Add(new Manufacturer() { ManufacturerName = manufacturerName })
      .Entity;

    await _db.SaveChangesAsync();

    return newManufacturer;
  }

  public async Task DeleteManufacturer(Manufacturer manufacturer)
  {
    _db.Manufacturers.Remove(manufacturer);

    await _db.SaveChangesAsync();
  }

  public async Task ChangeManufacturer(Manufacturer manufacturer, string changedManufacturerName)
  {
    manufacturer.ManufacturerName = changedManufacturerName;

    await _db.SaveChangesAsync();
  }

  public IEnumerable<ManufacturerDto> GetAllManufacturers() =>
    _db.Manufacturers.Select(m => new ManufacturerDto()
    {
      Id = m.Id,
      ManufacturerName = m.ManufacturerName
    });

  public IEnumerable<ManufacturerDto> GetRangeOfManufacturers(int limit, int page) =>
    _db.Manufacturers
      .Skip(page * limit)
      .Take(limit)
      .Select(m => new ManufacturerDto()
      {
        Id = m.Id,
        ManufacturerName = m.ManufacturerName
      });

  public int GetCountManufacturers() => _db.Manufacturers.Count();
}