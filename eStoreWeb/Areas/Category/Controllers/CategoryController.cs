using Microsoft.AspNetCore.Mvc;
using eStore.BL.Services;              
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using eStore.DAL.Models;
using eStore.Web.Areas.Customer.Models;
using eStore.BL.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace eStore.Web.Areas.Customer.Controllers
{
    [Area("Category")]
    [Route("/api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }


    // GET: api/categories
        [HttpGet]
        public async Task<ActionResult> GetAllCategories(int pageNumber = 1, int pageSize = 50)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page number must be greater than zero, and page size must be between 1 and 100.");
            }

            var (categories, totalRecords) = await _categoryService.GetAllCategories(pageNumber, pageSize);
            var categoryModels = _mapper.Map<IEnumerable<CategoryModel>>(categories);

            return Ok(new
            {
                TotalCount = totalRecords,
                Categories = categoryModels
            });
        }

        // GET: api/categories/list
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<object>>> GetCategoryList()
        {
            var (categories, _) = await _categoryService.GetAllCategories(1, int.MaxValue);
            var categoryList = categories.Select(c => new
            {
                Id = c.CategoryID,
                Name = c.CategoryName
            });

            return Ok(categoryList);
        }


        // GET: api/categories/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category.CategoryID != 0)
            {
                var categoryModel = _mapper.Map<CategoryModel>(category);
                return Ok(categoryModel);
            }
            return NotFound();
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult> AddCategory([FromBody] CategoryModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the incoming presentation model to a domain model.
                var category = _mapper.Map<CategoryDTO>(categoryModel);

                // Add the category through the business/service layer.
                int newCategoryId = await _categoryService.AddCategory(category);
                categoryModel.CategoryID = newCategoryId;

                if (newCategoryId != 0)
                {
                    return Ok(categoryModel);
                }
                else
                {
                    return Ok("Category name already exists");
                }
            }
            catch (Exception ex)
            {
                // Return a 500 error with a generic error message.
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  "An error occurred while processing your request.");
            }
        }


        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryModel categoryModel)
        {
            if (id != categoryModel.CategoryID)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the incoming presentation model to a domain model.
                var category = _mapper.Map<CategoryDTO>(categoryModel);

                // Update the category through the business/service layer.
                var updatedCategory = await _categoryService.UpdateCategory(category);

                if (updatedCategory.CategoryID != 0)
                {
                    return Ok(updatedCategory);
                }
                else
                {
                    return BadRequest("Category name already exists");
                }
            }
            catch (Exception ex)
            {
                // Return a 400 error with the exception message.
                return BadRequest(ex.Message);
            }
        }


        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }



    }
}
