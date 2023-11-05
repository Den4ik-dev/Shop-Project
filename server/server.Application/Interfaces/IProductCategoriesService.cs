using server.Domain.Dto;
using server.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace server.Application.Interfaces;
public interface IProductCategoriesService
{
  public Task<Category?> FindCategory(string categoryName);
  public Task<Category?> FindCategory(int categoryId);
  public Task<Category?> FindCategory(Expression<Func<Category, bool>> predicate);
  public Task<Category> AddCategory(string categoryName);
  public Task DeleteCategory(Category category);
  public Task ChangeCategory(Category category, string changedCategoryName);
  public IEnumerable<CategoryDto> GetAllCategories();
  public IEnumerable<CategoryDto> GetRangeOfCategories(int limit, int page);
  public int GetCountCategories();
}