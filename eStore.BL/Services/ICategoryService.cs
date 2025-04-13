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

        Task<int> AddCategory(CategoryDTO categoryModel);
        Task<CategoryDTO> UpdateCategory(CategoryDTO categoryModel);
        Task<int> DeleteCategory(int id);

        Task<(IEnumerable<CategoryDTO>, int)> GetAllCategories(int pageNumber, int pageSize);


    }
}
