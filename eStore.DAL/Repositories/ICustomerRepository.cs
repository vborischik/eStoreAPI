using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomers();
        Task<CustomerDTO> GetCustomerById(int id);
        Task<CustomerDTO> CheckCustomer(string email, string phone);
        Task<int> AddCustomer(CustomerDTO customer);
        Task<int> UpdateCustomer(CustomerDTO customer);
        Task<int> DeleteCustomer(int id);

    }
}
