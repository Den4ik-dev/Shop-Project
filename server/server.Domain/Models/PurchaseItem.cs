namespace server.Domain.Models;
public class PurchaseItem
{
  public int Id { get; set; }
  public int ProductCount { get; set; }
  public int ProductPrice { get; set; }

  public int ProductId { get; set; }
  public Product Product { get; set; }
  
  public int PurchaseId { get; set; }
  public Purchase Purchase { get; set; }
}