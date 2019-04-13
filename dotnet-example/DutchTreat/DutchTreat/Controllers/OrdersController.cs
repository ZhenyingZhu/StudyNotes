using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
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
    }
}