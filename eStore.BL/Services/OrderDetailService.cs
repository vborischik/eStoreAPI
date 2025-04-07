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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(
            IOrderDetailRepository orderDetailRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }

        public async Task<OrderDetailDTO> GetOrderDetailById(int id)
        {
            return await _orderDetailRepository.GetOrderDetailById(id);
        }

        public async Task<int> CreateOrderDetail(OrderDetailDTO orderDetail)
        {
            // Validate product exists
            var product = await _productRepository.GetProductById(orderDetail.ProductID);
            if (product == null || product.ProductID == 0)
            {
                throw new Exception($"Product with ID {orderDetail.ProductID} not found");
            }

            // Set the current price if not provided
            if (orderDetail.Price <= 0)
            {
                orderDetail.Price = product.Price;
            }

            return await _orderDetailRepository.AddOrderDetail(orderDetail);
        }

        public async Task<bool> UpdateOrderDetail(OrderDetailDTO orderDetail)
        {
            // Validate product exists
            var product = await _productRepository.GetProductById(orderDetail.ProductID);
            if (product == null || product.ProductID == 0)
            {
                throw new Exception($"Product with ID {orderDetail.ProductID} not found");
            }

            // Validate order detail exists
            var existingDetail = await _orderDetailRepository.GetOrderDetailById(orderDetail.OrderDetailID);
            if (existingDetail == null || existingDetail.OrderDetailID == 0)
            {
                return false;
            }

            var result = await _orderDetailRepository.UpdateOrderDetail(orderDetail);
            return result > 0;
        }

        public async Task<bool> DeleteOrderDetail(int id)
        {
            var result = await _orderDetailRepository.DeleteOrderDetail(id);
            return result > 0;
        }

        public async Task<bool> DeleteOrderDetailsByOrderId(int orderId)
        {
            var result = await _orderDetailRepository.DeleteOrderDetailsByOrderId(orderId);
            return result > 0;
        }
    }
}
