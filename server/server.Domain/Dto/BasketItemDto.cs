namespace server.Domain.Dto;
public class BasketItemDto
{
  public int Id { get; set; }
  public int ProductCount { get; set; }
  public int ProductId { get; set; }
  public int ProductUnitPrice { get; set; }
  public int CustomerId { get; set; }
}