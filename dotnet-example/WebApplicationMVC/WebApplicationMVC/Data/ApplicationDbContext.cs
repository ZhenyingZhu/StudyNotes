using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppTestModel> AppTestModel { get; set; }

        public DbSet<AppTestChildModel> AppTestChildModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppTestModel>().Property(t => t.AppTestInput).HasMaxLength(20);

            modelBuilder.Entity<AppTestModel>().HasData(new Models.AppTestModel() { Id = 10, AppTestInput = "Seeding Test1" });
        }
    }
}
