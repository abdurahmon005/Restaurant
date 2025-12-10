using Microsoft.AspNetCore.Mvc;
using WebApp.Aplication.Models.Products;
using WebApp.Aplication.Services.Interface;

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

        [HttpPost]
        public async Task<ActionResult<ResponseProductModel>> Create([FromForm] CreateProductModel dto)
        {
            var result = await _productService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseProductModel>> Update(int id, [FromForm] UpdateProductModel dto)
        {
            var result = await _productService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
                return NotFound();
           
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseProductModel>> GetById(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<ResponseProductModel>>> GetAll()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        //[HttpGet("category/{categoryId}")]
        //public async Task<ActionResult<List<ResponseProductModel>>> GetByCategory(int categoryId)
        //{
        //    var result = await _productService.GetByCategoryAsync(categoryId);
        //    return Ok(result);
        //}
    }
}
