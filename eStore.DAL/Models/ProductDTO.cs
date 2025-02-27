using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Models
{
    public class ProductDTO
    {
        public int ProductID { get; set; }        
        public string ProductName { get; set; }        
        public int CategoryID { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageURL { get; set; }
        public string UPC { get; set; }
        public string SKU { get; set; }
    }
}