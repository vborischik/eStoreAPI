using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Models
{
    public class OrderDetailDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Optional navigational properties for convenience
        public string ProductName { get; set; }
        public string SKU { get; set; }
    }
}
