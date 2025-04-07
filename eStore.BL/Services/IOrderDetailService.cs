using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsByOrderId(int orderId);
        Task<OrderDetailDTO> GetOrderDetailById(int id);
        Task<int> CreateOrderDetail(OrderDetailDTO orderDetail);
        Task<bool> UpdateOrderDetail(OrderDetailDTO orderDetail);
        Task<bool> DeleteOrderDetail(int id);
        Task<bool> DeleteOrderDetailsByOrderId(int orderId);
    }
}
