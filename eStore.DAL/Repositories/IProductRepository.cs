using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public interface IProductRepository
    {

        Task<IEnumerable<ProductDTO>> GetAllProducts();
        Task<IEnumerable<ProductDTO>> GetAllProducts(int pageNumber, int pageSize);
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> CheckProduct(string UPC, string SKU);
        Task<int> AddProduct(ProductDTO product);
        Task<int> UpdateProduct(ProductDTO product);
        Task<int> DeleteProduct(int id);       
        Task<int> GetTotalProductCount();

    }
}
