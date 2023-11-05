using server.Domain.Models;

namespace server.Domain.Dto;
public class ProductDto
{
  public int Id { get; set; }
  public string ProductName { get; set; }
  public string ProductDescription { get; set; }
  public int UnitPrice { get; set; }
  public int QuantityInStoke { get; set; }
  public int CategoryId { get; set; }
  public int ManufacturerId { get; set; }
  public IEnumerable<int> ImagesIds { get; set; }
  public IEnumerable<int> VideosIds { get; set; }
}

/*
  public int Id { get; set; }
  public string ProductName { get; set; }
  public string ProductDescription { get; set; }
  public int UnitPrice { get; set; }
  public int QuantityInStoke { get; set; }

  public int CategoryId { get; set; }
  public Category Category { get; set; }

  public int ManufacturerId { get; set; }
  public Manufacturer Manufacturer { get; set; }

  public List<BasketItem> BasketItems { get; set; }
  public List<PurchaseItem> Purchases { get; set; }

  public List<ProductImage> Images { get; set; }
  public List<ProductVideo> Videos { get; set; }
  public List<Delivery> Deliveries { get; set; }
  public List<PriceChange> PriceChanges { get; set; }
  public List<Comment> Commets { get; set; }
*/