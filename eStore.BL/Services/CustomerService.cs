using AutoMapper;
using eStore.DAL.Models;
using eStore.DAL.Repositories;


namespace eStore.BL.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
        }

        public async Task<CustomerDTO> GetCustomerById(int id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            return _mapper.Map<CustomerDTO>(customer);
        }

        public async Task<int> AddCustomer(CustomerDTO customerModel)
        {

            var existingCustomer = await _customerRepository.CheckCustomer(customerModel.Email,customerModel.Phone);

            if (existingCustomer != null && existingCustomer.CustomerID != customerModel.CustomerID && existingCustomer.CustomerID!=0)
            {
                return 0;// Indicating a conflict
            }

            return await _customerRepository.AddCustomer(customerModel);
        }

        public async Task<CustomerDTO> UpdateCustomer(CustomerDTO customerModel)
        {
            // Check if another customer has the same email or phone
            var existingCustomer = await _customerRepository.CheckCustomer(customerModel.Email, customerModel.Phone);

            if (existingCustomer != null && existingCustomer.CustomerID != customerModel.CustomerID && existingCustomer.CustomerID != 0)
            {
                return new CustomerDTO { CustomerID = 0 }; // Indicating a conflict
            }

            var updatedCustomerID = await _customerRepository.UpdateCustomer(customerModel);

            if (updatedCustomerID == 0)
            {
                // Handle the case where a duplicate email/phone exists (e.g., return an error response)
                return new CustomerDTO { CustomerID = 0 };
            }

            return customerModel;
        }


        public async Task<int> DeleteCustomer(int id)
        {
            return await _customerRepository.DeleteCustomer(id);
        }


    }
}
