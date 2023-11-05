namespace server.Domain.Dto;
public class CommentDto
{
  public int Id { get; set; }
  public string Content { get; set; }
  public DateTime CreationDate { get; set; }
  public int ProductId { get; set; }
  public int CustomerId { get; set; }
}
