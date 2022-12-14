using Microsoft.EntityFrameworkCore;
using ScrantonBranch.Entities;
using ScrantonBranch.Services;

namespace ScrantonBranch
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Order>().HasMany(p => p.products);
            builder.Entity<OrderProduct>().HasOne(p => p.product);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Message> Messages { get; set; }
        private DbSet<OrderProduct> OrderProducts { get; set; }

    }
}