using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Services.Interface;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateModel model)
        {
            var result = await _categoryService.CreateCategory(model);

            return Ok(ApiResult<CategoryResponceModel>.Success(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CategoryUpdateModel model)
        {
            var result = await _categoryService.UpdateAsync(id, model);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] CategoryDeleteModel model)
        {
            var result = await _categoryService.DeleteAsync(model);

            return Ok(result);
        }
    }
}
