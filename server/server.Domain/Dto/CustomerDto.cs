using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Domain.Dto;
public class CustomerDto
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public DateTime BirthDate { get; set; }
  public string Phone { get; set; }
  public string Address { get; set; }
  public string City { get; set; }
  public int Points { get; set; }
  public int Balance { get; set; }
  public string Email { get; set; }
}
