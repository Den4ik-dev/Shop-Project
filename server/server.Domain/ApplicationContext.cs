using Microsoft.EntityFrameworkCore;
using server.Domain.Models;

namespace server.Domain;
public class ApplicationContext : DbContext
{
  public DbSet<Customer> Customers => Set<Customer>();
  public DbSet<BasketItem> BasketItems => Set<BasketItem>();
  public DbSet<Purchase> Purchases => Set<Purchase>();
  public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
  public DbSet<Product> Products => Set<Product>();
  public DbSet<ProductImage> ProductImages => Set<ProductImage>();
  public DbSet<ProductVideo> ProductVideos => Set<ProductVideo>();
  public DbSet<Comment> Comments => Set<Comment>();
  public DbSet<PriceChange> PriceChanges => Set<PriceChange>();
  public DbSet<Delivery> Deliveries => Set<Delivery>();
  public DbSet<Category> Categories => Set<Category>();
  public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
  public DbSet<Role> Roles => Set<Role>();

  public ApplicationContext(DbContextOptions<ApplicationContext> options)
    : base(options)
  {
    //Database.EnsureCreated();
  }
}