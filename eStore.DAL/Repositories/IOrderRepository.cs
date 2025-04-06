using eStore.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<OrderDTO> GetOrderById(int id);
        Task<int> AddOrder(OrderDTO order);
        Task<int> UpdateOrder(OrderDTO order);
        Task<int> DeleteOrder(int id);
    }
}
