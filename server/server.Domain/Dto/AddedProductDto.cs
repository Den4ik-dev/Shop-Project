namespace server.Domain.Dto;
public class AddedProductDto
{
  public string ProductName { get; set; }
  public string ProductDescription { get; set; }
  public int UnitPrice { get; set; }
  public int QuantityInStoke { get; set; }
  public int CategoryId { get; set; }
  public int ManufacturerId { get; set; }
}