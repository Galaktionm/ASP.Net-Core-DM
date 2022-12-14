using BookStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProductService.DTO;
using ProductService.Entities;

namespace ProductService.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseService dbContext;
        public ProductController(DatabaseService dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> getAllProducts()
        {
            return Ok(await dbContext.GetAsync());
        }
        
        private async Task<ActionResult<Product>> Get(string id)
        {
            var product = await dbContext.GetAsync(id);      

            if (product is null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<List<Product>>> GetByName(string name)
        {
            var product = await dbContext.GetByNameAsync(name);

            if (product != null)
            {
                return Ok(product);
            }

            return BadRequest();
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> addProduct(Product product)
        {
            try
            {
                dbContext.CreateAsync(product);
                return CreatedAtAction(nameof(Get), new { id = product.id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:length(24)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> updateProduct(String id, ProductUpdateRequest request)
        {
            var product = await dbContext.GetAsync(id);

            if (product != null)
            {
                product.manufacturer = request.manufacturer;
                product.unitPrice = request.unitPrice;
                product.available = request.available;
                await dbContext.UpdateAsync(id, product);
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await dbContext.GetAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            await dbContext.RemoveAsync(id);

            return NoContent();
        }


    }
}
