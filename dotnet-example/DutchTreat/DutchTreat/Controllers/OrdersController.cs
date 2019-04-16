using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(this._repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var order = this._repository.GetOrderById(id);
                if (order != null) return Ok(order);
                else return NotFound();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Failed to get an order: {ex}");
                return BadRequest("Failed to get an order");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody]OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newOrder = new Order()
                    {
                        OrderDate = model.OrderDate,
                        OrderNumber = model.OrderNumber,
                        Id = model.OrderId
                    };

                    if (newOrder.OrderDate == DateTime.MinValue)
                    {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    this._repository.AddEntity(newOrder);
                    if (this._repository.SaveAll())
                    {
                        var vm = new OrderViewModel()
                        {
                            OrderDate = newOrder.OrderDate,
                            OrderNumber = newOrder.OrderNumber,
                            OrderId = newOrder.Id
                        };

                        return Created($"api/orders/{vm.OrderId}", vm);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Failed to save a new order: {ex}");
            }
            return BadRequest("Failed to save a new order");
        }
    }
}