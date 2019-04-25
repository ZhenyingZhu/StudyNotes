using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager)
        {
            this._ctx = ctx;
            this._hosting = hosting;
            this._userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUser user = await this._userManager.FindByEmailAsync("zhenying@dutchtreat.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "zhenying",
                    LastName = "zhu",
                    Email = "zhenying@dutchtreat.com",
                    UserName = "zhenying@dutchtreat.com"
                };

                var result = await this._userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create a new user in seeder.");
                }
            }

            if (!_ctx.Orders.Any())
            {
                Order testOrder = new Order()
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "#1"
                };
                _ctx.Orders.Add(testOrder);

                _ctx.SaveChanges();
            }

            if (!_ctx.Products.Any())
            {
                var filepath = Path.Combine(this._hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                //_ctx.Products.AddRange(products);
                // zhenying: trying to figure out which data exceed size.
                foreach (var product in products)
                {
                    _ctx.Products.Add(product);
                    try
                    {
                        _ctx.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }

                _ctx.SaveChanges();
            }

            var order = _ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();
            if (order != null && (order.Items == null || !order.Items.Any()))
            {
                order.User = user;
                order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = _ctx.Products.First(),
                            Quantity = 5,
                            UnitPrice = _ctx.Products.First().Price
                        }
                    };

                _ctx.SaveChanges();
            }

        }
    }
}
