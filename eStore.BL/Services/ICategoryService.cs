using eStore.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStore.BL.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(int id);

        Task<int> AddCategory(CategoryDTO customerModel);
        Task<CategoryDTO> UpdateCategory(CategoryDTO customerModel);
        Task<int> DeleteCategory(int id);
    }
}
