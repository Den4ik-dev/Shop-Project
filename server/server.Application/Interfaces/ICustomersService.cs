using server.Domain.Dto;
using server.Domain.Models;
using System.Linq.Expressions;

namespace server.Application.Interfaces;
public interface ICustomersService
{
  public Task<Customer?> FindCustomer(Expression<Func<Customer, bool>> predicate);
  public Task<Customer?> FindCustomer(string email);
  public Task<Customer?> FindCustomer(LoginCustomerDto loginCustomer);
  public Task<Customer> AddCustomer(RegisteredCustomerDto registeredCustomerDto);
  public Task SetCustomerRefreshToken(Customer customer, string refreshToken);
  public Task<Role> GetCustomerRole(Customer customer);
  public Task ChangeCustomer(Customer customer, ChangedCustomerDto changedCustomer);
  public Task DeleteCustomer(Customer customer);
}
