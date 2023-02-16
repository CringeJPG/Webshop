using KWRWebShopAPI.Database.Entities;
using KWRWebShopAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KWRWebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                List<CategoryResponse> category = await _categoryService.GetAllCategory();

                if(category.Count == 0)
                {
                    return NoContent();
                }
                return Ok(category);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int categoryId)
        {
            try
            {
                var categoryResponse = await _categoryService.GetCategoryById(categoryId);

                if (categoryResponse == null)
                {
                    return NotFound();
                }

                return Ok(categoryResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryRequest newCategory)
        {
            try
            {
                CategoryResponse categoryResponse = await _categoryService.CreateCategory(newCategory);
                return Ok(categoryResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{categoryId}")]
        public async Task<IActionResult> UpdateCategory([FromRoute]int categoryId, [FromBody]CategoryRequest updateCategory)
        {
            try
            {
                var categoryResponse = await _categoryService.UpdateCategory(categoryId, updateCategory);

                if (categoryResponse == null)
                {
                    return NotFound();
                }
                return Ok(categoryResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute]int categoryId)
        {
            try
            {
                var categoryResponse = await _categoryService.DeleteCategory(categoryId);

                if (categoryResponse == null)
                {
                    return NotFound();
                }
                return Ok(categoryResponse);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
