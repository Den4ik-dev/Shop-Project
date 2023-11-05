using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Domain.Dto;
public class LoginCustomerDto
{
  public string Email { get; set; }
  public string Password { get; set; }
}