using eStore.DAL.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
   public interface ICustomerService
{
    Task<IEnumerable<CustomerDTO>> GetAllCustomers();
    Task<CustomerDTO> GetCustomerById(int id);
              
    Task<int> AddCustomer(CustomerDTO customerModel);
    Task<CustomerDTO> UpdateCustomer(CustomerDTO customerModel);
    Task<int> DeleteCustomer(int id);
    Task<CustomerDTO> CheckCustomer(string email, string phone);
    Task<(IEnumerable<CustomerDTO>, int)> GetAllCustomers(int pageNumber, int pageSize); // New method for pagination 

    }
}
