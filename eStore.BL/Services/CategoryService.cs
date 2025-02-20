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

        public Task<int> AddCategory(CategoryDTO customerModel)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }



        public Task<CategoryDTO> GetCategoryById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO> UpdateCategory(CategoryDTO customerModel)
        {
            throw new NotImplementedException();
        }

        //public CategoryService(ICategoryRepository categoryrRepository, IMapper mapper)
        //{
        //    _categoryRepository = categoryRepository;
        //    _mapper = mapper;
        //}

        //public async Task<IEnumerable<CustomerDTO>> GetAllCategories()
        //{
        //    var customers = await _categoryRepository.GetAllCategories();
        //    return _mapper.Map<IEnumerable<CategoryDTO>>(category);
        //}

        //public async Task<CustomerDTO> GetCategoryById(int id)
        //{
        //    var customer = await _categoryRepository.GetCategoryById(id);
        //    return _mapper.Map<CategoryDTO>(category);
        //}

        //public async Task<int> AddCategory(CategoryDTO categoryModel)
        //{

        //    var existingCategory = await _categoryRepository.CheckCategory(categoryModel.CategoryID, categoryModel.CategoryName);

        //    if (existingCategory != null && existingCategory.CategoryID != categoryModel.CategoryID && existingCategory.CategoryID != 0)
        //    {
        //        return 0;// Indicating a conflict
        //    }

        //    return await _categoryRepository.AddCategory(categoryModel);
        //}

        //public async Task<CustomerDTO> UpdateCategory(CategoryDTO categoryModel)
        //{
        //    // Check if another customer has the same email or phone
        //    var existingCategory = await _categoryRepository.CheckCategory(categoryModel.CategoryID, categoryModel.CategoryName);

        //    if (existingCategory != null && existingCategory.CategoryID != categoryModel.CategoryID && existingCategory.CategoryID != 0)
        //    {
        //        return new CategoryDTO { CategoryID = 0 }; // Indicating a conflict
        //    }

        //    var updatedCategoryID = await _categoryRepository.UpdateCategory(categoryModel);

        //    if (updatedCategoryID == 0)
        //    {
        //        // Handle the case where a duplicate email/phone exists (e.g., return an error response)
        //        return new CategoryDTO { CategoryID = 0 };
        //    }

        //    return categoryModel;
        //}


        //public async Task<int> DeleteCategory(int id)
        //{
        //    return await _categoryRepository.DeleteCategory(id);
        //}
    }
}
