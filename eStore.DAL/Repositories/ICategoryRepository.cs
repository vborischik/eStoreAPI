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
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(int id);
        Task<CategoryDTO> CheckCategory(string categoryName);
        Task<int> AddCategory(CategoryDTO category);
        Task<int> UpdateCategory(CategoryDTO category);
        Task<int> DeleteCategory(int id);
    }
}
