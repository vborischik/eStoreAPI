using AutoMapper;
using eStore.DAL.Models;
using eStore.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        
        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<OrderDTO> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        public async Task<(int orderId, string errorMessage)> AddOrder(OrderDTO order)
        {
            // Business validations for orders can be added here.
            int id = await _orderRepository.AddOrder(order);
            return (id, null);
        }

        public async Task<OrderDTO> UpdateOrder(OrderDTO order)
        {
            int updated = await _orderRepository.UpdateOrder(order);
            return updated > 0 ? order : new OrderDTO { OrderID = 0 };
        }

        public async Task<int> DeleteOrder(int id)
        {
            return await _orderRepository.DeleteOrder(id);
        }
    }
}
