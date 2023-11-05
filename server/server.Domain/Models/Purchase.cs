namespace server.Domain.Models;
public class Purchase
{
  public int Id { get; set; }
  public DateTime PurchaseDate { get; set; }
  public int Price { get; set; }

  public int CustomerId { get; set; }
  public Customer Customer { get; set; }

  public List<PurchaseItem> PurchaseItems { get; set; }
}