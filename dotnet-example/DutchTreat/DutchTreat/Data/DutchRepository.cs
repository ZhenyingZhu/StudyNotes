using DutchTreat.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;

        public DutchRepository(DutchContext ctx)
        {
            this._ctx = ctx;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return this._ctx.Products.OrderBy(p => p.Title).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return this._ctx.Products.Where(p => p.Category == category).ToList();
        }

        public bool SaveAll()
        {
            return this._ctx.SaveChanges() > 0;
        }
    }
}
