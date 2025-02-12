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

        public Task<int> AddCategory(CategoryDTO customer)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO> CheckCategory(string email, string phone)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategory()
        {
            var sql = "SELECT * FROM Category";
            return await QueryAsync<CategoryDTO>(sql);
        }

        public Task<CategoryDTO> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateCategory(CategoryDTO customer)
        {
            throw new NotImplementedException();
        }
    }
}
