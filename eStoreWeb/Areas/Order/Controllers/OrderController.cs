using AutoMapper;
using eStore.BL.Services;
using eStore.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using eStore.Web.Areas.Order.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace eStore.Web.Areas.Order.Controllers
{
    [Area("Order")]
    [Route("/api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IOrderDetailService orderDetailService, IMapper mapper)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }

        #region Order Operations

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult> GetAllOrders(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page number must be greater than zero, and page size must be between 1 and 100.");
            }

            // Retrieve paginated order list
            var (orders, totalRecords) = await _orderService.GetAllOrders(pageNumber, pageSize);

            // Map domain models to presentation models
            var orderModels = _mapper.Map<IEnumerable<OrderModel>>(orders);

            // Return paginated response with total count
            return Ok(new
            {
                TotalCount = totalRecords,
                Orders = orderModels
            });
        }

        // GET: api/orders/{id}
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

        // GET: api/orders/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetOrdersByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomer(customerId);
            var orderModels = _mapper.Map<IEnumerable<OrderModel>>(orders);
            return Ok(orderModels);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the presentation model to a domain model
                var order = _mapper.Map<OrderDTO>(orderModel);

                // Create the order through the business layer
                int newOrderId = await _orderService.CreateOrder(order);

                if (newOrderId != 0)
                {
                    // Get the complete order with details
                    var createdOrder = await _orderService.GetOrderById(newOrderId);
                    var createdOrderModel = _mapper.Map<OrderModel>(createdOrder);

                    return Ok(createdOrderModel);
                }
                else
                {
                    return BadRequest("Failed to create order");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request: " + ex.Message);
            }
        }
               
        // PUT: api/orders/{id}/status
        [HttpPut("{id}/status")]   
        public async Task<ActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusModel model)
        {
            if (id != model.OrderID)
            {
                return BadRequest("Order ID mismatch between URL and request body");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Validate that status is a valid enum value
                string[] validStatuses = { "Pending", "Processing", "Completed", "Cancelled" };
                if (!validStatuses.Contains(model.OrderStatus.ToString()))
                {
                    return BadRequest($"Invalid status: {model.OrderStatus}. Valid statuses are: {string.Join(", ", validStatuses)}");
                }

                var success = await _orderService.UpdateOrderStatus(id, model.OrderStatus.ToString());

                if (success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to update order status");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request: " + ex.Message);
            }
        }
        
        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                var success = await _orderService.DeleteOrder(id);

                if (success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Order Detail Operations

        // GET: api/orders/{orderId}/details
        [HttpGet("{orderId}/details")]
        public async Task<ActionResult<IEnumerable<OrderDetailModel>>> GetOrderDetails(int orderId)
        {
            var orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
            var orderDetailModels = _mapper.Map<IEnumerable<OrderDetailModel>>(orderDetails);
            return Ok(orderDetailModels);
        }

        // GET: api/orders/details/{id}
        [HttpGet("details/{id}")]
        public async Task<ActionResult<OrderDetailModel>> GetOrderDetailById(int id)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailById(id);
            if (orderDetail != null && orderDetail.OrderDetailID != 0)
            {
                var orderDetailModel = _mapper.Map<OrderDetailModel>(orderDetail);
                return Ok(orderDetailModel);
            }
            return NotFound();
        }

        // POST: api/orders/{orderId}/details
        [HttpPost("{orderId}/details")]
      
        public async Task<ActionResult> AddOrderDetail(int orderId, [FromBody] OrderDetailModel orderDetailModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the order ID from the URL matches the one in the model
            orderDetailModel.OrderID = orderId;

            try
            {
                // Map the presentation model to a domain model
                var orderDetail = _mapper.Map<OrderDetailDTO>(orderDetailModel);

                // Create the order detail through the business layer
                int newOrderDetailId = await _orderDetailService.CreateOrderDetail(orderDetail);

                if (newOrderDetailId != 0)
                {
                    // Get the complete order detail
                    var createdOrderDetail = await _orderDetailService.GetOrderDetailById(newOrderDetailId);
                    var createdOrderDetailModel = _mapper.Map<OrderDetailModel>(createdOrderDetail);

                    return Ok(createdOrderDetailModel);
                }
                else
                {
                    return BadRequest("Failed to create order detail");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request: " + ex.Message);
            }
        }

        // PUT: api/orders/details/{id}
        [HttpPut("details/{id}")]
     
        public async Task<ActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailModel orderDetailModel)
        {
            if (id != orderDetailModel.OrderDetailID)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var orderDetail = _mapper.Map<OrderDetailDTO>(orderDetailModel);
                var success = await _orderDetailService.UpdateOrderDetail(orderDetail);

                if (success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request: " + ex.Message);
            }
        }

        // DELETE: api/orders/details/{id}
        [HttpDelete("details/{id}")]
        public async Task<ActionResult> DeleteOrderDetail(int id)
        {
            try
            {
                var success = await _orderDetailService.DeleteOrderDetail(id);

                if (success)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    }
}