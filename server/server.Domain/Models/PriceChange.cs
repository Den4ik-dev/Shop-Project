namespace server.Domain.Models;
public class PriceChange
{
  public int Id { get; set; }
  public int NewPrice { get; set; }
  public DateTime DatePriceChange { get; set; } = DateTime.Now;

  public int ProductId { get; set; }
  public Product Product { get; set; }
}