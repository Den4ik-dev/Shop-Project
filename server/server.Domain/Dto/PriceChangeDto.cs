namespace server.Domain.Dto;
public class PriceChangeDto
{
  public int Id { get; set; }
  public int NewPrice { get; set; }
  public DateTime DatePriceChange { get; set; } = DateTime.Now;
  public int ProductId { get; set; }
}