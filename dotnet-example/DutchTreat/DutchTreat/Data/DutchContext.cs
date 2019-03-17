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
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
