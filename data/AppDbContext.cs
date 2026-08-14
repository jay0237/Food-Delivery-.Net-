using FoodOrderingSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Food> Foods { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }



protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Cart>()
        .HasOne(c => c.User)
        .WithOne()
        .HasForeignKey<Cart>(c => c.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<CartItem>()
        .HasOne(ci => ci.Cart)
        .WithMany(c => c.Items)
        .HasForeignKey(ci => ci.CartId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<CartItem>()
        .HasOne(ci => ci.Food)
        .WithMany()
        .HasForeignKey(ci => ci.FoodId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<CartItem>()
        .HasIndex(ci => new { ci.CartId, ci.FoodId })
        .IsUnique();
}
}
