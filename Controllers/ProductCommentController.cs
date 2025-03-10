using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;
using SimoshStoreAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public ProductCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet("/api/confirm-comment/{id}")]
        public async Task<IActionResult> ConfirmProductComment(int id)
        {
            try
            {
                var result = await _commentService.ConfirmProductComment(id);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("/api/productcomments/user/{userId}")]
        public async Task<IActionResult> GetProductCommentsByUserId(int userId)
        {
            try
            {
                var comments = await _commentService.GetProductCommentsByUserId(userId);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("/api/productcomments")]
        public async Task<IActionResult> GetProductComments()
        {
            try
            {
                var comments = await _commentService.GetProductComments();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("/api/productcomments/{id}")]
        public async Task<IActionResult> GetProductComment(int id)
        {
            try
            {
                var comment = await _commentService.GetProductCommentById(id);
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("/api/delete/productcomment/{id}")]
        public async Task<IActionResult> DeleteProductComment(int id)
        {
            try
            {
                var result = await _commentService.DeleteProductComment(id);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("/api/update/productcomment{id}")]
        public async Task<IActionResult> UpdateProductCommentEntity(ProductCommentDTO dto, int id)
        {
            try
            {
                var result = await _commentService.UpdateProductComment(dto, id);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("/api/create/productcomment")]
        public async Task<IActionResult> CreateProductComment([FromBody] ProductCommentDTO dto)
        {
            var result = await _commentService.CreateProductCommentAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
