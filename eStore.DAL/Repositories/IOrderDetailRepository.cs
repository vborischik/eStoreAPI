using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId);
        Task<OrderDetailDTO> GetOrderDetailById(int id);
        Task<int> AddOrderDetail(OrderDetailDTO orderDetail);
        Task<int> UpdateOrderDetail(OrderDetailDTO orderDetail);
        Task<int> DeleteOrderDetail(int id);
        Task<int> DeleteOrderDetailsByOrderId(int orderId);


    }
}
