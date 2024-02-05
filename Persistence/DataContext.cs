using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<ProductName> ProductNames { get; set; }
    public DbSet<ProductUpdateHistory> ProductUpdateHistories { get; set; }
    public DbSet<SerialNumberHistory> SerialNumberHistories { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //builder.Entity<Product>(p =>
        //{
        //    p.Property(x => x.SerialNumber)
        //    .UseCollation("SQL_Latin1_General_CP1_CS_AS");
        //});
        //builder.Entity<ProductName>(p =>
        //{
        //    p.Property(x => x.Name)
        //    .UseCollation("SQL_Latin1_General_CP1_CS_AS");
        //});
        //builder.Entity<Location>(p =>
        //{
        //    p.Property(x => x.Name)
        //    .UseCollation("SQL_Latin1_General_CP1_CS_AS");
        //});
    }
}

