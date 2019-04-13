using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _ctx;
        private readonly IHostingEnvironment _hosting;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting)
        {
            this._ctx = ctx;
            this._hosting = hosting;
        }

        public void Seed()
        {
            _ctx.Database.EnsureCreated();

            if (!_ctx.Orders.Any())
            {
                Order testOrder = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "#1"
                };
                _ctx.Orders.Add(testOrder);
            }

            if (!_ctx.Products.Any())
            {
                var filepath = Path.Combine(this._hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var existOrder = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (existOrder != null)
                {
                    existOrder.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                _ctx.SaveChanges();
            }

            var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
            if (order != null && (order.Items == null || !order.Items.Any()))
            {
                order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = _ctx.Products.First(),
                            Quantity = 5,
                            UnitPrice = _ctx.Products.First().Price
                        }
                    };
            }

            _ctx.SaveChanges();
        }
    }
}
