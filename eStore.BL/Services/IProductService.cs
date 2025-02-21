using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public interface IProductService
    {

        Task<IEnumerable<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<(int productId, string errorMessage)> AddProduct(ProductDTO productModel);
        Task<ProductDTO> UpdateProduct(ProductDTO productModel);
        Task<int> DeleteProduct(int id);

    }
}
