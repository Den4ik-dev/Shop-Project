using Microsoft.EntityFrameworkCore;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace server.Infrastructure.Services.Customers;
public class CustomersService : ICustomersService
{
  private ApplicationContext _db;
  public CustomersService(ApplicationContext db) =>
    _db = db;

  public async Task<Customer?> FindCustomer(LoginCustomerDto loginCustomer) =>
    await _db.Customers.FirstOrDefaultAsync(c =>
      c.Email == loginCustomer.Email &&
      c.Password == Convert.ToHexString(SHA512.Create().ComputeHash(
        Encoding.UTF8.GetBytes(loginCustomer.Password))));
  public async Task<Customer?> FindCustomer(Expression<Func<Customer, bool>> predicate) =>
    await _db.Customers.FirstOrDefaultAsync(predicate);
  public async Task<Customer?> FindCustomer(string email) =>
    await _db.Customers.FirstOrDefaultAsync(c => c.Email == email);
  public async Task<Customer> AddCustomer(RegisteredCustomerDto registeredCustomerDto)
  {
    Customer newCustomer = _db.Customers.Add(new Customer()
    {
      FirstName = registeredCustomerDto.FirstName,
      LastName = registeredCustomerDto.LastName,
      BirthDate = registeredCustomerDto.BirthDate,
      Phone = registeredCustomerDto.Phone,
      Address = registeredCustomerDto.Address,
      City = registeredCustomerDto.City,
      Email = registeredCustomerDto.Email,
      Password = Convert.ToHexString(SHA512.Create().ComputeHash(
        Encoding.UTF8.GetBytes(registeredCustomerDto.Password))),
    }).Entity;
    await _db.SaveChangesAsync();

    return newCustomer;
  }

  public async Task SetCustomerRefreshToken(Customer customer, string refreshToken)
  {
    customer.RefreshToken = refreshToken;
    customer.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

    await _db.SaveChangesAsync();
  }

  public async Task<Role> GetCustomerRole(Customer customer) =>
    await _db.Roles.FirstAsync(r => r.Id == customer.RoleId);

  public async Task ChangeCustomer(Customer customer, ChangedCustomerDto changedCustomer)
  {
    customer.FirstName = changedCustomer.FirstName;
    customer.LastName = changedCustomer.LastName;
    customer.BirthDate = changedCustomer.BirthDate;
    customer.Phone = changedCustomer.Phone;
    customer.Address = changedCustomer.Address;
    customer.City = changedCustomer.City;
    customer.Email = changedCustomer.Email;
    customer.Password = Convert.ToHexString(SHA512.Create().ComputeHash(
      Encoding.UTF8.GetBytes(changedCustomer.Password)));

    await _db.SaveChangesAsync();
  }

  public async Task DeleteCustomer(Customer customer)
  {
    _db.Customers.Remove(customer);

    await _db.SaveChangesAsync();
  }
}