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
    public class CategoryRepository : BaseDAL, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration, string connectionName) : base(configuration, connectionName)
        {
        }

        public async Task<int> AddCategory(CategoryDTO category)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", category.CategoryName);
            parameters.Add("p_CategoryID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await ExecuteAsync("AddCustomer", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("p_CategoryID");


        }

        public async Task<CategoryDTO> CheckCategory(string categoryName)
        {
            var sql = "SELECT * FROM Categories WHERE CategoryName = @categoryName LIMIT 1;";

            return await QuerySingleAsync<CategoryDTO>(sql, new { CategoryName = categoryName}) ?? new CategoryDTO();
        }

        public async Task<int> DeleteCategory(int id)
        {
            var sql = "DELETE FROM Categories WHERE CategoryID = @Id";
            return await ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var sql = "SELECT * FROM Categories";
            return await QueryAsync<CategoryDTO>(sql);
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var sql = "SELECT * FROM Categories WHERE CategoryID = @Id";
            return await QuerySingleAsync<CategoryDTO>(sql, new { Id = id }) ?? new CategoryDTO();
        }

        public async Task<int> UpdateCategory(CategoryDTO category)
        {
            var parameters = new DynamicParameters();
            parameters.Add("p_Category", category.CategoryID, DbType.Int32);
            parameters.Add("p_FirstName", category.CategoryName, DbType.String);

            var t = await ExecuteAsync(
                "UpdateCategory",
                parameters,
                commandType: CommandType.StoredProcedure
            );


            return t;
        }
    }
}
