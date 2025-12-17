using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models.Categories;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Services.Interface;
using WebApp.Domain.Entities;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("Create Products")]
        public async Task<ActionResult<ResponseProductModel>> Create([FromForm] CreateProductModel dto)
        {
            var result = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("Update Products{id}")]
        public async Task<ActionResult<ResponseProductModel>> Update(int id, [FromForm] UpdateProductModel dto)
        {
            var result = await _productService.UpdateAsync(id, dto);
            return Ok(result);
        }

       

        [HttpGet("Get Products{id}")]
        public async Task<ActionResult<ResponseProductModel>> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("Get All")]
        public async Task<ActionResult<List<ResponseProductModel>>> GetAll()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("Get by Category/{categoryId}")]
        public async Task<ActionResult<List<ResponseProductModel>>> GetByCategory(int categoryId)
        {
            var result = await _productService.GetByIdAsync(categoryId);

            return Ok(result);
        }

        [HttpDelete("Delete products")]

        public async Task<ActionResult<CategoryResponceModel>>Delete([FromQuery] DeleteProductModel dto)
        {
            var result = await _productService.DeleteAsync(dto);

            return Ok(result);
        }
    }
}
