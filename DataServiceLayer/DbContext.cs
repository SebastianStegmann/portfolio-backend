using Microsoft.EntityFrameworkCore;
using System;


namespace DataServiceLayer;
public class NorthwindContext : DbContext
{
    // public DbSet<Category> Categories { get; set; }
    // public DbSet<Product> Products { get; set; }
    // public DbSet<Order> Orders { get; set; }
    // public DbSet<OrderDetail> OrdersDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseNpgsql("host=localhost;db=northwind;uid=postgres;pwd=postgres");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
//         modelBuilder.Entity<Category>().ToTable("categories");
//         modelBuilder.Entity<Category>().Property(x => x.Id).HasColumnName("categoryid");
//         modelBuilder.Entity<Category>().Property(x => x.Name).HasColumnName("categoryname");
//         modelBuilder.Entity<Category>().Property(x => x.Description).HasColumnName("description");
//
//
//         modelBuilder.Entity<Product>().ToTable("products");
//         modelBuilder.Entity<Product>().Property(x => x.Id).HasColumnName("productid");
//         modelBuilder.Entity<Product>().Property(x => x.Name).HasColumnName("productname");
//         modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnName("unitprice");
//         modelBuilder.Entity<Product>().Property(x => x.CategoryId).HasColumnName("categoryid");
//         modelBuilder.Entity<Product>().Property(x => x.QuantityPerUnit).HasColumnName("quantityperunit");
//         modelBuilder.Entity<Product>().Property(x => x.UnitsInStock).HasColumnName("unitsinstock");
//
//
//         modelBuilder.Entity<Order>().ToTable("orders");
//         modelBuilder.Entity<Order>().Property(x => x.Id).HasColumnName("orderid");
//         modelBuilder.Entity<Order>().Property(x => x.Date).HasColumnName("orderdate");
//         modelBuilder.Entity<Order>().Property(x => x.Required).HasColumnName("requireddate");
//         modelBuilder.Entity<Order>().Property(x => x.ShipName).HasColumnName("shipname");
//         modelBuilder.Entity<Order>().Property(x => x.ShipCity).HasColumnName("shipcity");
//
//         modelBuilder.Entity<OrderDetails>().ToTable("orderdetails");
//         modelBuilder.Entity<OrderDetails>()
//           .HasKey(od => new { od.OrderId, od.ProductId }); // Composite key
// modelBuilder.Entity<OrderDetails>()
//     .Property(od => od.OrderId).HasColumnName("orderid");
// modelBuilder.Entity<OrderDetails>()
//     .Property(od => od.ProductId).HasColumnName("productid");
//         modelBuilder.Entity<OrderDetails>()
//           .HasOne(od => od.Product)
//           .WithMany()
//           .HasForeignKey(od => od.ProductId)
//           .HasConstraintName("FK_orderdetails_products");
//
//         modelBuilder.Entity<OrderDetails>()
//           .HasOne(od => od.Order)
//           .WithMany(o => o.OrderDetails)
//           .HasForeignKey(od => od.OrderId)
//           .HasConstraintName("FK_orderdetails_orders");

    }



}

