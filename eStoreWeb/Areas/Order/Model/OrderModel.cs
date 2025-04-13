using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eStore.Web.Areas.Order.Models
{
    public class OrderModel
    {
        public OrderModel()
        {
            OrderDetails = new List<OrderDetailModel>();
            OrderStatus = eStore.Web.Areas.Order.Models.OrderStatus.Pending.ToString();
            OrderDate = DateTime.Now;
        }

        public int OrderID { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerID { get; set; }

        public string? CustomerName { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }

        public List<OrderDetailModel> OrderDetails { get; set; }
    }

    public class OrderDetailModel
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        // Optional properties
        public string ProductName { get; set; } = "";
        public string SKU { get; set; } = "";
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }

    public class UpdateOrderStatusModel
    {
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        public OrderStatus OrderStatus { get; set; }
    }
}