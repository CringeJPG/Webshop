using KWRWebShopAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KWRWebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomProductsAsync()
        {
            try
            {
                List<ProductResponse> products = await _productService.GetRandomProductsAsync();

                if (products.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            try
            {
                List<ProductResponse> products = await _productService.GetAllProductsAsync();

                if (products.Count() == 0)
                {
                    return NoContent();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProductByIdAsync([FromRoute] int productId)
        {
            try
            {
                var productResponse = await _productService.GetProductByIdAsync(productId);

                if (productResponse == null)
                {
                    return NotFound();
                }
                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductRequest newProduct)
        {
            try
            {
                // Attempts to create a new product using the provided ProductRequest object.
                ProductResponse productResponse = await _productService.CreateProductAsync(newProduct);

                // Returns an Ok result with the created ProductResponse object.
                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                // If an error occurred, return a Problem result with the result message.
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int productId)
        {
            try
            {
                var productResponse = await _productService.DeleteByIdAsync(productId);

                if(productResponse == null)
                {
                    return NotFound();
                }
                return Ok(productResponse);

            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateProductAsync([FromRoute] int productId, [FromBody] ProductRequest updateProduct)
        {
            try
            {
                var productResponse = await _productService.UpdateProductAsync(productId, updateProduct);

                if (productResponse == null)
                {
                    return NotFound();
                }
                return Ok(productResponse);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
