using BoostBayApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BoostBayApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // kezdő adatok
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Turbó", Description = "Turbófeltöltők" },
                new Category { Id = 2, Name = "Motor belső", Description = "Dugattyú, hajtókar, csapágy" },
                new Category { Id = 3, Name = "Kipufogó", Description = "Leömlő, straight pipe" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Garrett G30-770 turbó", SpecValue = "+140 LE", Price = 489000, Stock = 5, CategoryId = 1 },
                new Product { Id = 2, Name = "Kovácsolt dugattyú szett", SpecValue = "11,5 bar", Price = 238000, Stock = 6, CategoryId = 2 },
                new Product { Id = 3, Name = "Versenycsapágy szett", SpecValue = "0,025 mm", Price = 68000, Stock = 12, CategoryId = 2 },
                new Product { Id = 4, Name = "Tuning leömlő 4-2-1", SpecValue = "+12 LE", Price = 179000, Stock = 4, CategoryId = 3 },
                new Product { Id = 5, Name = "Straight pipe 76 mm", SpecValue = "76 mm", Price = 94000, Stock = 9, CategoryId = 3 }
            );
        }
    }
}
