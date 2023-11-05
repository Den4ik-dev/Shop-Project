using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Infrastructure.Services;
public class ProductCategoriesService : IProductCategoriesService
{
  private ApplicationContext _db;
  public ProductCategoriesService(ApplicationContext db) =>
    _db = db;

  public async Task<Category?> FindCategory(string categoryName) =>
    await _db.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);

  public async Task<Category?> FindCategory(int categoryId) =>
    await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
  public async Task<Category?> FindCategory(Expression<Func<Category, bool>> predicate) =>
    await _db.Categories.FirstOrDefaultAsync(predicate);

  public async Task<Category> AddCategory(string categoryName)
  {
    Category newCategory = 
      _db.Categories.Add(new Category() { CategoryName = categoryName })
      .Entity;

    await _db.SaveChangesAsync();

    return newCategory;
  }

  public async Task DeleteCategory(Category category)
  {
    _db.Categories.Remove(category);

    await _db.SaveChangesAsync();
  }

  public async Task ChangeCategory(Category category, string changedCategoryName)
  {
    category.CategoryName = changedCategoryName;

    await _db.SaveChangesAsync();
  }

  public IEnumerable<CategoryDto> GetAllCategories() =>
    _db.Categories.Select(c => new CategoryDto() 
    { 
      Id = c.Id, 
      CategoryName = c.CategoryName 
    });

  public IEnumerable<CategoryDto> GetRangeOfCategories(int limit, int page) =>
    _db.Categories
      .Skip(page * limit)
      .Take(limit)
      .Select(c => new CategoryDto()
      {
        Id = c.Id,
        CategoryName = c.CategoryName
      });

  public int GetCountCategories() => _db.Categories.Count();
}