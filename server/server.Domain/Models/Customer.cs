namespace server.Domain.Models;
public class Customer
{
  public int Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public DateTime BirthDate { get; set; }
  public string Phone { get; set; }
  public string Address { get; set; }
  public string City { get; set; }
  public int Points { get; set; }
  public int Balance { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }

  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }

  public List<BasketItem> Basket { get; set; }
  public List<Purchase> Purchases { get; set; }

  public int RoleId { get; set; } = 1;
  public Role Role { get; set; }
}