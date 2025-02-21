using Dapper;
using eStore.DAL.eStore.DAL;
using eStore.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public class ProductRepository :BaseDAL, IProductRepository
        {
        public ProductRepository(IConfiguration configuration, string connectionName) : base(configuration, connectionName)
        {
        }

        public async Task<int> AddProduct(ProductDTO product)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@p_ProductName", product.ProductName);
            parameters.Add("@p_CategoryID", product.CategoryID);
            parameters.Add("@p_Price", product.Price);
            parameters.Add("@p_StockQuantity", product.StockQuantity);
            parameters.Add("@p_ImageURL", product.ImageURL);
            parameters.Add("@p_SKU", product.SKU);
            parameters.Add("@p_UPC", product.UPC);
            parameters.Add("@p_ProductID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await ExecuteAsync("AddProduct", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("p_ProductID");
        }

        public async Task<ProductDTO> CheckProduct(string UPC, string SKU)
        {
            var sql = "SELECT * FROM Products WHERE UPC = @UPC OR SKU = @SKU LIMIT 1;";
            return await QuerySingleAsync<ProductDTO>(sql, new { UPC, SKU }) ?? new ProductDTO();
        }

        public async Task<int> DeleteProduct(int id)
        {
            var sql = "DELETE FROM Products WHERE ProductID = @Id";
            return await ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
                var sql = "SELECT * FROM Products";
           
            return await QueryAsync<ProductDTO>(sql);
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var sql = "SELECT * FROM Products WHERE ProductID = @Id";
            return await QuerySingleAsync<ProductDTO>(sql, new { Id = id }) ?? new ProductDTO();
        }

        public async Task<int> UpdateProduct(ProductDTO product)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_ProductID", product.ProductID, DbType.Int32);
            parameters.Add("p_ProductName", product.ProductName ?? "", DbType.String);
            parameters.Add("p_CategoryID", product.CategoryID, DbType.Int32);
            parameters.Add("p_Price", product.Price, DbType.Decimal, precision: 10, scale: 2);
            parameters.Add("p_StockQuantity", product.StockQuantity, DbType.Int32);
            parameters.Add("p_ImageURL", product.ImageURL ?? "", DbType.String);
            parameters.Add("p_SKU", product.SKU ?? "", DbType.String);
            parameters.Add("p_UPC", product.UPC ?? "", DbType.String);


            var result = await ExecuteAsync(
                "UpdateProduct",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }
}
