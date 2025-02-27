using AutoMapper;
using eStore.BL.Helpers;
using eStore.BL.Services;
using eStore.DAL.Models;
using eStore.Web.Areas.Product.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStore.Web.Areas.Product.Controllers
{
    [Area("Admin")]
    [Route("/api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts()
        {
            // Retrieve domain models from BL service.
            var products = await _productService.GetAllProducts();
            // Map them to presentation models.
            var productModels = _mapper.Map<IEnumerable<ProductModel>>(products);
            return Ok(productModels);
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product != null && product.ProductID != 0)
            {
                var productModel = _mapper.Map<ProductModel>(product);
                return Ok(productModel);
            }
            return NotFound();
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate SKU and UPC formats if needed
      
            try
            {
                // Map the incoming presentation model to a domain model.
                var product = _mapper.Map<ProductDTO>(productModel);

                // Add the product through the business/service layer.
                var (newProductId, errorMessage) = await _productService.AddProduct(product);

                if (newProductId != 0)
                {
                    productModel.ProductID = newProductId;
                    return Ok(productModel);
                }
                else
                {
                    // Return the specific error message from the service
                    return BadRequest(errorMessage);
                }
            }
            catch (Exception ex)
            {
                // Return a 500 error with a generic error message.
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "An error occurred while processing your request.");
            }
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductModel productModel)
        {
            if (id != productModel.ProductID)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // If any of the custom validations failed, return 400.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the incoming presentation model to a domain model.
                var product = _mapper.Map<ProductDTO>(productModel);

                // Update the product through the business/service layer.
                var updatedProduct = await _productService.UpdateProduct(product);

                if (updatedProduct.ProductID != 0)
                {
                    return Ok(updatedProduct);
                }
                else
                {
                    return BadRequest("SKU or UPC already in use by another product");
                }
            }
            catch (Exception ex)
            {
                // Return a 400 error with the exception message.
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


    }
}
