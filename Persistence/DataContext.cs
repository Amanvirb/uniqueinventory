using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<PartNumber> PartNumbers { get; set; }
    public DbSet<ProductUpdateHistory> ProductUpdateHistories { get; set; }
    public DbSet<SerialNumberHistory> SerialNumberHistories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }

}
