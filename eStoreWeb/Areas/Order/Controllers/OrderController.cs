using AutoMapper;
using eStore.BL.Services;
using eStore.DAL.Models;
using eStore.Web.Areas.Order.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStore.Web.Areas.Order.Controllers
{
    [Area("Admin")]
    [Route("/api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        
        // GET: /api/orders
        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            var orderModels = _mapper.Map<IEnumerable<OrderModel>>(orders);
            return Ok(orderModels);
        }
        
        // GET: /api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order != null && order.OrderID != 0)
            {
                var orderModel = _mapper.Map<OrderModel>(order);
                return Ok(orderModel);
            }
            return NotFound();
        }
        
        // POST: /api/orders
        [HttpPost]
        public async Task<ActionResult> AddOrder([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                var orderDTO = _mapper.Map<OrderDTO>(orderModel);
                var (newOrderId, errorMessage) = await _orderService.AddOrder(orderDTO);
                if (newOrderId != 0)
                {
                    orderModel.OrderID = newOrderId;
                    return Ok(orderModel);
                }
                return BadRequest(errorMessage);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }
        
        // PUT: /api/orders/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderModel orderModel)
        {
            if (id != orderModel.OrderID)
                return BadRequest("ID mismatch");
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                var orderDTO = _mapper.Map<OrderDTO>(orderModel);
                var updatedOrder = await _orderService.UpdateOrder(orderDTO);
                if (updatedOrder.OrderID != 0)
                    return Ok(updatedOrder);
                return BadRequest("Update failed");
            }
            catch
            {
                return BadRequest("Update failed");
            }
        }
        
        // DELETE: /api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
