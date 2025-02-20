using System.ComponentModel.DataAnnotations;

namespace eStore.Web.Areas.Customer.Models
{
    public class CategoryModel
    {

        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        public string CategoryName { get; set; }



    }
}
