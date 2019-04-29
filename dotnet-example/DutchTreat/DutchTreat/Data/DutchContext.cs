using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchContext : IdentityDbContext<StoreUser>
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
            modelBuilder.Entity<Product>().Property(p => p.Title).HasMaxLength(250);
            // following https://stackoverflow.com/questions/3504660/decimal-precision-and-scale-in-ef-code-first,
            // but doesn't have this method.
            //modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);

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
