using BackendApi.Models.enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApi.Models
{
    public class StoreDBContext : IdentityDbContext
    {
        public StoreDBContext()
        {

        }

        public StoreDBContext(DbContextOptions<StoreDBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.Property(e => e.OrderId).ValueGeneratedNever().ValueGeneratedOnAdd();
                entity.Property(e => e.DeliveryDate).HasColumnType("date");
                entity.Property(e => e.OrderDate).HasColumnType("date");
                entity.Property(e => e.CreditCardExpirationDate).HasColumnType("date");
                entity.Property(e => e.TotalCost).HasColumnType("money");
                entity.Property(e => e.OrderStatus).HasColumnType("int");
                entity.Property(e => e.CreditCardNumber).HasMaxLength(16).IsFixedLength(true);

                entity.HasOne(d => d.ShippingInfo)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShippingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_ShippingInfo");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_User");
            });

            builder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId });
                entity.ToTable("OrderProduct");
                entity.Property(e => e.OrderId).HasColumnName("OrderId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderProduct_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderProduct_Product");
            });

            builder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductId).ValueGeneratedNever().ValueGeneratedOnAdd();
                entity.Property(e => e.Price).HasColumnType("money");
            });

            builder.Entity<ShippingInfo>(entity =>
            {
                entity.ToTable("ShippingInfo");
                entity.HasKey(e => e.ShippingInfoId);
                entity.Property(e => e.ShippingInfoId).ValueGeneratedNever().ValueGeneratedOnAdd();
                entity.Property(e => e.ShippingCost).HasColumnType("money");
            });
            
            builder.Entity<ShippingInfo>().HasData(
                new ShippingInfo() { ShippingInfoId = 1, ShippingMethod = ShippingMethod.AirPlane, ShippingCost = 1000 },
                new ShippingInfo() { ShippingInfoId = 2, ShippingMethod = ShippingMethod.Fairy, ShippingCost = 750 },
                new ShippingInfo() { ShippingInfoId = 3, ShippingMethod = ShippingMethod.Train, ShippingCost = 500 },
                new ShippingInfo() { ShippingInfoId = 4, ShippingMethod = ShippingMethod.Truck, ShippingCost = 250 });

            builder.Entity<Product>().HasData(
                new Product() { ProductId = 1, ProductName = "AKM", Price = 500, Quantity = 50 },
                new Product() { ProductId = 2, ProductName = "M4", Price = 650, Quantity = 50 },
                new Product() { ProductId = 3, ProductName = "P90", Price = 400, Quantity = 50 },
                new Product() { ProductId = 4, ProductName = "Pistol", Price = 150, Quantity = 250 },
                new Product() { ProductId = 5, ProductName = "AWM", Price = 2000, Quantity = 10 });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }

    }
}
