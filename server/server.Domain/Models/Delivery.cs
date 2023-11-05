namespace server.Domain.Models;
public class Delivery
{
  public int Id { get; set; }
  public int ProductCount { get; set; }
  public DateTime DeliveryDate { get; set; } = DateTime.Now;

  public int ProductId { get; set; }
  public Product Product { get; set; }
}
