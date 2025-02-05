using System.ComponentModel.DataAnnotations;

namespace eStore.Web.Areas.Customer.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string Email { get; set; }
    }
}
