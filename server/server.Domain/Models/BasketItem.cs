namespace server.Domain.Models;
public class BasketItem
{
  public int Id { get; set; }
  public int ProductCount { get; set; }

  public int ProductId { get; set; }
  public Product Product { get; set; }

  public int CustomerId { get; set; }
  public Customer Customer { get; set; }
}