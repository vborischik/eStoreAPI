using Microsoft.AspNetCore.Mvc;
using eStore.BL.Services;              
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStore.DAL.Models;
using eStore.Web.Areas.Customer.Models;
using eStore.BL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace eStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("/api/customers")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult> GetAllCustomers(int pageNumber = 1, int pageSize = 50)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page number must be greater than zero, and page size must be between 1 and 100.");
            }

            //// Retrieve paginated customer list
            var (customers, totalRecords) = await _customerService.GetAllCustomers(pageNumber, pageSize);

            //// Map domain models to presentation models
            var customerModels = _mapper.Map<IEnumerable<CustomerModel>>(customers);

            //// Return paginated response with total count
            return Ok(new
            {
                TotalCount = totalRecords,
                Customers = customerModels
            });
        }


        //// GET: api/customers
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAllCustomers()
        //{
        //    //// Retrieve domain models from BL service.
        //    var customers = await _customerService.GetAllCustomers();
        //    //// Map them to presentation models.
        //    var customerModels = _mapper.Map<IEnumerable<CustomerModel>>(customers);
        //    return Ok(customerModels);
        //}


        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            if (customer != null)
            {
                var customerModel = _mapper.Map<CustomerModel>(customer);
                return Ok(customerModel);
            }
                return NotFound();
        }

        // POST: api/customers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCustomer([FromBody] CustomerModel customerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ValidationHelper.IsValidEmail(customerModel.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }

            if (!ValidationHelper.IsValidPhone(customerModel.Phone))
            {
                ModelState.AddModelError("Phone", "Invalid phone format.");
            }

            // If any of the custom validations failed, return 400.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                // Map the incoming presentation model to a domain model.
                var customer = _mapper.Map<CustomerDTO>(customerModel);

                // Add the customer through the business/service layer.
                int newCustomerId = await _customerService.AddCustomer(customer);
                customerModel.CustomerID = newCustomerId;


                if (newCustomerId != 0)
                {
                    return Ok(customerModel);
                }
                else
                {
                    return Ok("email or phone already used");
                }
                               
               
            }
            catch (Exception ex)          
            { 
                
                // _logger.LogError(ex, "Error while adding a customer");

                // Return a 500 error with a generic error message.
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "An error occurred while processing your request.");
            }


        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateCustomer(int id, [FromBody] CustomerModel customerModel)
        {
            if (id != customerModel.CustomerID)
            {
                return BadRequest("ID mismatch");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ValidationHelper.IsValidEmail(customerModel.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format.");
            }

            if (!ValidationHelper.IsValidPhone(customerModel.Phone))
            {
                ModelState.AddModelError("Phone", "Invalid phone format.");
            }

            // If any of the custom validations failed, return 400.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            try
            {
                // Map the incoming presentation model to a domain model.
                var customer = _mapper.Map<CustomerDTO>(customerModel);

                // Add the customer through the business/service layer.
                var updatedcustomer = await _customerService.UpdateCustomer(customer);

                if (updatedcustomer.CustomerID!=0) {
                    return Ok(updatedcustomer);
                }
                else
                {
                    return BadRequest("Phone or Email already used");
                }
                // Here, assuming that after creation, customer.CustomerID is populated.
                
            }
            catch (Exception ex)
            {
             
                // Return a 500 error with a generic error message.
                return BadRequest(ex.Message);
            }

        }
       
        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomer(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
            
            
        }
    }
}
