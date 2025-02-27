using AutoMapper;
using eStore.DAL.Models;
using eStore.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public ProductService(IProductRepository productRepository,  ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }


        public async Task<(int productId, string errorMessage)> AddProduct(ProductDTO productModel)
        {
            // Check if product with same UPC or SKU already exists
            var existingProduct = await _productRepository.CheckProduct(productModel.UPC, productModel.SKU);
            if (existingProduct != null && existingProduct.ProductID != 0)
            {
                return (0, "SKU or UPC already in use");
            }

            // Check if the category exists
            var category = await _categoryRepository.GetCategoryById(productModel.CategoryID);
            if (category == null || category.CategoryID == 0)
            {
                return (0, "Category does not exist");
            }

            // Only add product if both checks pass
            int id = await _productRepository.AddProduct(productModel);
            return (id, null);
        }

        public async Task<int> DeleteProduct(int id)
        {
            return await _productRepository.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateProduct(ProductDTO productModel)
        {
            // Check if another product has the same SKU or UPC
            var existingProduct = await _productRepository.CheckProduct(productModel.UPC, productModel.SKU);
            if (existingProduct != null && existingProduct.ProductID != productModel.ProductID && existingProduct.ProductID != 0)
            {
                return new ProductDTO { ProductID = 0 }; // Indicating a conflict
            }

            var updatedProductID = await _productRepository.UpdateProduct(productModel);
            if (updatedProductID == 0)
            {
                // Handle the case where update failed
                return new ProductDTO { ProductID = 0 };
            }
            return productModel;
        }
    }
}
