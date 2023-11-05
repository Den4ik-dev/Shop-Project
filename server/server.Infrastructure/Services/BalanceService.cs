using server.Application.Interfaces;
using server.Domain;
using server.Domain.Models;

namespace server.Infrastructure.Services;
public class BalanceService : IBalanceService
{
  private ApplicationContext _db;
  public BalanceService(ApplicationContext db, ICustomersService customersService)
  {
    _db = db;
  }

  public async Task SetBalance(Customer customer, int balance)
  {
    customer.Balance += balance;

    await _db.SaveChangesAsync();
  }

  public async Task IncreaseBalance(Customer customer, int money)
  {
    customer.Balance += money;

    await _db.SaveChangesAsync();
  }

  public async Task ReductionBalance(Customer customer, int money)
  {
    customer.Balance -= money;

    await _db.SaveChangesAsync();
  }

  public async Task ClearBalance(Customer customer)
  {
    customer.Balance = 0;

    await _db.SaveChangesAsync();
  }
}