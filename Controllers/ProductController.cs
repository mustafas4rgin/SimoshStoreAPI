using System.Security.Cryptography;
using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;
using SimoshStoreAPI;

namespace MyApp.Namespace
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
        [HttpGet("/api/search-product")]
        public async Task<IActionResult> Search()
        {
            var model = await _productService.SearchProduct();

            return Ok(model);
        }
        [HttpGet("/api/update-admin/product/{id}")]
        public async Task<IActionResult> AdminUpdateProduct(int id)
        {
            var dto = await _productService.AdminUpdate(id);

            return Ok(dto);
        }
        [HttpGet("/api/popularproducts")]
        public async Task<IActionResult> PopularProducts([FromQuery]int? take)
        {
            var popularProducts = await _productService.PopularProductsAsync(take);
            return Ok(popularProducts);
        }

        [HttpGet("/api/bestproducts")]
        public async Task<IActionResult> BestProducts()
        {
            var products = await _productService.BestProductsAsync();
            return Ok(products);
        }
        [HttpGet("/api/products")]
        public async Task<IActionResult> GetProducts()
        {
            try 
            {
                var products = await _productService.GetProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("/api/create/product")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO product)
        {
            var result = await _productService.CreateProductAsync(product);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(product);
        }
        [HttpDelete("/api/delete/product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [HttpPut("/api/update/product/{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO product,[FromRoute] int id)
        {
            var result = await _productService.UpdateProductAsync(product,id);
            if(!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(product);
        }
        [HttpGet("/api/products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var productDetailsViewModel = await _productService.GetProductByIdAsync(id);
            if(productDetailsViewModel is null)
            {
                return NotFound("Product not found");
            }
            return Ok(productDetailsViewModel);
        }
        [HttpGet("/api/product-list")]
        public async Task<IActionResult> GetProductList()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(new ListProductsViewModel
            {
                productEntities = products.ToList()
            });
        }
    }
}
