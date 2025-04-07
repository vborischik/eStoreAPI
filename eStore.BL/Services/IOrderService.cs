using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrders();
        Task<(IEnumerable<OrderDTO>, int)> GetAllOrders(int pageNumber, int pageSize);
        Task<OrderDTO> GetOrderById(int id);
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomer(int customerId);
        Task<int> CreateOrder(OrderDTO orderModel);
        Task<bool> UpdateOrderStatus(int orderId, string status);
        Task<bool> DeleteOrder(int id);
    }
}
