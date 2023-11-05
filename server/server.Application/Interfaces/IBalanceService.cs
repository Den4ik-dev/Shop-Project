using server.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Application.Interfaces;
public interface IBalanceService
{
  public Task SetBalance(Customer customer, int balance);
  public Task IncreaseBalance(Customer customer, int money);
  public Task ReductionBalance(Customer customer, int money);
  public Task ClearBalance(Customer customer);
}
