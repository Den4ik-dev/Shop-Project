namespace server.Domain.Models;
public class Comment
{
  public int Id { get; set; }
  public string Content { get; set; }
  public DateTime CreationDate { get; set; } = DateTime.Now;

  public int ProductId { get; set; }
  public Product Product { get; set; }

  public int CustomerId { get; set; }
  public Customer Customer { get; set; }
}