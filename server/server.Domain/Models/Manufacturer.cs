namespace server.Domain.Models;
public class Manufacturer
{
  public int Id { get; set; }
  public string ManufacturerName { get; set; }

  public List<Product> Products { get; set; }
}
