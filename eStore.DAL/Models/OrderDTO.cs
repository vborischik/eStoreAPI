using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Models
{

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }

    public class OrderDTO
    {
        public OrderDTO()
        {
            OrderDate = DateTime.Now;
            OrderStatus = OrderStatus.Pending;
            OrderDetails = new List<OrderDetailDTO>();
        }

        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }        
        public OrderStatus OrderStatus { get; set; }// "Pending", "Processing", "Completed", "Cancelled"
        public List<OrderDetailDTO> OrderDetails { get; set; }

    }
}
