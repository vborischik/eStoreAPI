using AutoMapper;
using eStore.DAL.Models;
using eStore.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IMapper _mapper;

        public OrderService(
             IOrderRepository orderRepository,
             IProductRepository productRepository,
             IOrderDetailService orderDetailService,
             IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderDetailService = orderDetailService;
            _mapper = mapper;
        }

        public async Task<int> CreateOrder(OrderDTO orderModel)
        {
            // Validate customer exists (could add this check)

            // Calculate total amount based on product prices
            decimal totalAmount = 0;

            foreach (var detail in orderModel.OrderDetails)
            {
                // Get current product price
                var product = await _productRepository.GetProductById(detail.ProductID);
                if (product == null || product.ProductID == 0)
                {
                    throw new Exception($"Product with ID {detail.ProductID} not found");
                }

                // Set the price from the current product price
                detail.UnitPrice = product.Price;

                // Add to total
                totalAmount += detail.UnitPrice * detail.Quantity;
            }

            // Update the order total
            orderModel.TotalAmount = totalAmount;

            // Create the order
            return await _orderRepository.AddOrder(orderModel);
        }

        public async Task<bool> DeleteOrder(int id)
        {
            // You might want to add validation logic, e.g., only allow deletion if status is "Pending"
            var order = await _orderRepository.GetOrderById(id);
            if (order == null || order.OrderID == 0)
            {
                return false;
            }

            var result = await _orderRepository.DeleteOrder(id);
            return result > 0;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return orders;
        }

        public async Task<(IEnumerable<OrderDTO>, int)> GetAllOrders(int pageNumber, int pageSize)
        {
            var orders = await _orderRepository.GetAllOrders(pageNumber, pageSize);
            var totalRecords = await _orderRepository.GetTotalOrderCount();

            return (orders, totalRecords);
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomer(int customerId)
        {
            return await _orderRepository.GetOrdersByCustomer(customerId);
        }

        public async Task<bool> UpdateOrderStatus(int orderId, string status)
        {
            // Validate the status is one of the allowed values
            string[] validStatuses = { "Pending", "Processing", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status))
            {
                throw new ArgumentException($"Invalid order status: {status}");
            }

            var result = await _orderRepository.UpdateOrderStatus(orderId, status);
            return result > 0;
        }

        public async Task<int?> UpdateOrder(OrderDTO order)
        {
            decimal totalAmount = 0;

            // Get original order and details from DB
            var existingOrder = await _orderRepository.GetOrderById(order.OrderID);
            if (existingOrder == null || existingOrder.OrderID == 0)
                return null;

            var existingDetails = existingOrder.OrderDetails;
            var updatedDetails = order.OrderDetails;

            // 1. Detect removed details
            var deletedDetails = existingDetails
                .Where(existing => !updatedDetails.Any(updated => updated.OrderDetailID == existing.OrderDetailID))
                .ToList();

            foreach (var deleted in deletedDetails)
            {
                await _orderDetailService.DeleteOrderDetail(deleted.OrderDetailID);
            }

            // 2. Process updated and new details
            foreach (var detail in updatedDetails)
            {
                var product = await _productRepository.GetProductById(detail.ProductID);
                if (product == null)
                    throw new Exception($"Product with ID {detail.ProductID} not found");

                detail.UnitPrice = product.Price;
                totalAmount += detail.UnitPrice * detail.Quantity;

                if (detail.OrderDetailID == 0)
                {
                    detail.OrderID = order.OrderID;
                    await _orderDetailService.CreateOrderDetail(detail);
                }
                else
                {
                    await _orderDetailService.UpdateOrderDetail(detail);
                }
            }

            // 3. Update order header
            order.TotalAmount = totalAmount;
            var result = await _orderRepository.UpdateOrder(order);

            return result > 0 ? order.OrderID : (int?)null;
        }


    }
}
