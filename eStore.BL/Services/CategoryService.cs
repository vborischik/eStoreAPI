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
    public class CategoryService: ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var category = await _categoryRepository.GetAllCategories();
                return _mapper.Map<IEnumerable<CategoryDTO>>(category);
        }

        public async Task<int> AddCategory(CategoryDTO categoryModel)
        {

            var existingCategory= await _categoryRepository.CheckCategory(categoryModel.CategoryName);

            if (existingCategory != null && existingCategory.CategoryID != categoryModel.CategoryID && categoryModel.CategoryID != 0)
            {
                return 0;// Indicating a conflict
            }


            return await _categoryRepository.AddCategory(categoryModel);
        }

        public async Task<int> DeleteCategory(int id)
        {
            return await _categoryRepository.DeleteCategory(id);
        }


        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> UpdateCategory(CategoryDTO categoryModel)
        {
            // Check if another category has the same name
            var existingCategory = await _categoryRepository.CheckCategory(categoryModel.CategoryName);
            if (existingCategory != null && existingCategory.CategoryID != categoryModel.CategoryID && existingCategory.CategoryID != 0)
            {
                return new CategoryDTO { CategoryID = 0 }; // Indicating a conflict
            }

            var updatedCategoryID = await _categoryRepository.UpdateCategory(categoryModel);
            if (updatedCategoryID == 0)
            {
                // Handle the case where update failed
                return new CategoryDTO { CategoryID = 0 };
            }

            return categoryModel;
        }

    }
}
