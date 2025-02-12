using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.DAL.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategory();
        Task<CategoryDTO> GetCategoryById(int id);
        Task<CategoryDTO> CheckCategory(string email, string phone);
        Task<int> AddCategory(CategoryDTO customer);
        Task<int> UpdateCategory(CategoryDTO customer);
        Task<int> DeleteCategory(int id);
    }
}
