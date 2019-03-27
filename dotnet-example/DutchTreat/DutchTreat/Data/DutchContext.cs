using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchContext : DbContext
    {
        public DutchContext(DbContextOptions<DutchContext> options): base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // The right way to use it.
            modelBuilder.Entity<Product>().Property(p => p.Title).HasMaxLength(50);

            // Use it to seeding data.
            modelBuilder.Entity<Order>().HasData(new Order()
            {
                Id = 1,
                OrderDate = DateTime.UtcNow,
                OrderNumber = "12345"
            });
        }
    }
}
