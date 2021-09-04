using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public ApplicationDbContext()
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>()
                .HasIndex(e => e.CustomerCode)
                .IsUnique();
            builder.Entity<Unit>()
                .HasIndex(e => e.Name)
                .IsUnique();
            builder.Entity<Product>()
                .HasIndex(e => e.ProductCode)
                .IsUnique();
            builder.Entity<Order>()
                .HasIndex(e => e.OrderNo)
                .IsUnique();
            base.OnModelCreating(builder);

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
