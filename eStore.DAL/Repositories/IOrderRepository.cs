using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public interface IOrderRepository
    {

        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<IEnumerable<OrderDTO>> GetAllOrders(int pageNumber, int pageSize);
        Task<int> GetTotalOrderCount();
        Task<OrderDTO> GetOrderById(int id);
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomer(int customerId);
        Task<int> AddOrder(OrderDTO order);
        Task<int> UpdateOrderStatus(int orderId, string status);
        Task<int> DeleteOrder(int id);

    }
}
