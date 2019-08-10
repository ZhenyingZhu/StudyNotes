using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _ctx;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext ctx, ILogger<DutchRepository> logger)
        {
            this._ctx = ctx;
            this._logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                this._logger.LogInformation("GetAllProducts was called");

                return this._ctx.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"GetAllProducts was failed: {ex}");
                return null;
            }
            
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return this._ctx.Products.Where(p => p.Category == category).ToList();
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return this._ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList();
            }
            else
            {
                return this._ctx.Orders.ToList();
            }
            
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return this._ctx.Orders.Where(o => o.User.UserName == username).Include(o => o.Items).ThenInclude(i => i.Product).ToList();
            }
            else
            {
                return this._ctx.Orders.Where(o => o.User.UserName == username).ToList();
            }

        }

        public Order GetOrderById(string username, int id)
        {
            return this._ctx.Orders.Where(o => o.Id == id && o.User.UserName == username).Include(o => o.Items).ThenInclude(i => i.Product).FirstOrDefault();
        }

        public void AddOrder(Order newOrder)
        {
            foreach (var item in newOrder.Items)
            {
                item.Product = _ctx.Products.Find(item.Product.Id);
            }

            AddEntity(newOrder);
        }

        public void AddEntity(object model)
        {
            this._ctx.Add(model);
        }

        public bool SaveAll()
        {
            return this._ctx.SaveChanges() > 0;
        }
    }
}
