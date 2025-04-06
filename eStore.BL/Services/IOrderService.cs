using eStore.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<OrderDTO> GetOrderById(int id);
        Task<(int orderId, string errorMessage)> AddOrder(OrderDTO order);
        Task<OrderDTO> UpdateOrder(OrderDTO order);
        Task<int> DeleteOrder(int id);
    }
}