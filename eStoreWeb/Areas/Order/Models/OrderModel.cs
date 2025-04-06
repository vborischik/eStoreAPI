using System;
using System.ComponentModel.DataAnnotations;

namespace eStore.Web.Areas.Order.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        public decimal TotalAmount { get; set; }

        public string? OrderStatus { get; set; }
    }
}
