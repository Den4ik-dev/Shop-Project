using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.Domain.Dto;
public class DeliveryDto
{
  public int Id { get; set; }
  public int ProductCount { get; set; }
  public DateTime DeliveryDate { get; set; }
  public int ProductId { get; set; }
}
