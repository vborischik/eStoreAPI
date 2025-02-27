using System.ComponentModel.DataAnnotations;

namespace eStore.Web.Areas.Product.Models
{
    public class ProductModel
    {

        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public int CategoryID { get; set; }
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "UPC is required.")]
        public string UPC { get; set; }

        [Required(ErrorMessage = "SKU name is required.")]
        public string SKU { get; set; }

    }
}



