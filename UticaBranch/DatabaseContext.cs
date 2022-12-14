using Microsoft.EntityFrameworkCore;
using UticaBranch.Entities;
using UticaBranch.Services;

namespace UticaBranch
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

        private DbSet<OrderProduct> OrderProducts { get; set; }

        private DbSet<Message> Messages { get; set; }

    }
}
