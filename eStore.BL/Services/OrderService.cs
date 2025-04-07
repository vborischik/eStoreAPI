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
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
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
                detail.Price = product.Price;

                // Add to total
                totalAmount += detail.Price * detail.Quantity;
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
    }
}
