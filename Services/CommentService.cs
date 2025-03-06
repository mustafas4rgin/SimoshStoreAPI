using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SimoshStore;

namespace SimoshStoreAPI;

public class CommentService : ICommentService
{
    private readonly IDataRepository _dataRepository;
    public CommentService(IDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }
    public async Task<IEnumerable<ProductCommentEntity>> GetProductComments()
    {
        var comments = await _dataRepository.GetAll<ProductCommentEntity>()
                            .Include(c => c.Product)
                            .Include(c => c.User)
                            .ToListAsync();
        if(comments is null)
        {
            throw new Exception("No comments found");
        }
        return comments;
    }
    public async Task<ProductCommentEntity> GetProductCommentById(int id)
    {
        var comment = await _dataRepository.GetAll<ProductCommentEntity>()
                            .Include(c => c.Product)
                            .Include(c => c.User)
                            .FirstOrDefaultAsync(c => c.Id == id);
        if(comment is null)
        {
            throw new Exception("Comment not found");
        }
        return comment;
    }
    public async Task<IServiceResult> DeleteProductComment(int id)
    {
        var comment = await _dataRepository.GetByIdAsync<ProductCommentEntity>(id);
        if(comment is null)
        {
            return new ServiceResult(false, "Comment not found");
        }
        await _dataRepository.DeleteAsync<ProductCommentEntity>(id);
        return new ServiceResult(true, "Comment deleted successfully");
    }
    public async Task<IServiceResult> UpdateProductComment(ProductCommentDTO dto, int id)
    {
        var comment = await _dataRepository.GetByIdAsync<ProductCommentEntity>(id);
        if(comment is null)
        {
            return new ServiceResult(false, "Comment not found");
        }
        comment.Text = dto.Text;
        if(dto.StarCount > 5 || dto.StarCount < 0)
        {
            return new ServiceResult(false, "Star count must be between 0 and 5");
        }
        comment.StarCount = dto.StarCount;
        comment.IsConfirmed = dto.IsConfirmed;
        await _dataRepository.UpdateAsync(comment);
        return new ServiceResult(true, "Comment updated successfully");
    }
    public async Task<IEnumerable<BlogCommentEntity>> GetBlogComments()
    {
        var comments = await _dataRepository.GetAll<BlogCommentEntity>().ToListAsync();
        if(comments is null)
        {
            throw new Exception("No comments found");
        }
        if(comments.Count == 0)
        {
            throw new Exception("No comments found");
        }
        return comments;
    }
    public async Task<BlogCommentEntity> GetBlogCommentById(int id)
    {
        var comment = await _dataRepository.GetByIdAsync<BlogCommentEntity>(id);
        if(comment is null)
        {
            throw new Exception("Comment not found");
        }
        return comment;
    }
    public async Task<IServiceResult> DeleteBlogComment(int id)
    {
        var comment = await _dataRepository.GetByIdAsync<BlogCommentEntity>(id);
        if(comment is null)
        {
            return new ServiceResult(false, "Comment not found");
        }
        await _dataRepository.DeleteAsync<BlogCommentEntity>(id);
        return new ServiceResult(true, "Comment deleted successfully");
    }
    public async Task<IServiceResult> UpdateBlogComment(BlogCommentDTO dto, int id)
    {
        var comment = await _dataRepository.GetByIdAsync<BlogCommentEntity>(id);
        if(comment is null)
        {
            return new ServiceResult(false, "Comment not found");
        }
        comment.Comment = dto.Comment;
        comment.BlogId = dto.BlogId;
        comment.Email = dto.Email;
        comment.Name = dto.Name;
        await _dataRepository.UpdateAsync(comment);
        return new ServiceResult(true, "Comment updated successfully");
    }
           
}
