using System.Data;
using App.Data.Entities;
using SimoshStoreAPI;

namespace SimoshStore;

public class MappingHelper
{
    public static UserEntity RegisterDtoToUserEntity(RegisterDto dto)
    {
        HashingHelper.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        return new UserEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            ResetToken = "",
            ResetTokenExpires = DateTime.UtcNow
        };
    }
    public static BlogCommentEntity MappingBlogCommentEntity(BlogCommentDTO dto)
    {
        return new BlogCommentEntity
        {
            Comment = dto.Comment,
            Name = dto.Name,
            Email = dto.Email,
            BlogId = dto.BlogId
        };
    }
    public static ProductCommentEntity MappingProductCommentEntity(ProductCommentDTO dto)
    {
        return new ProductCommentEntity
        {
            Text = dto.Text,
            StarCount = dto.StarCount,
            ProductId = dto.ProductId,
            UserId = dto.UserId,
            IsConfirmed = false
        };
    }

    public static OrderEntity MappingOrderEntity(OrderDTO dto)
    {
        return new OrderEntity
        {
            OrderCode = dto.OrderCode,
            UserId = dto.UserId
        };
    }
    public static BlogCategoryEntity MappingBlogCategoryEntity(BlogCategoryEntityDTO dto)
    {
        return new BlogCategoryEntity
        {
            Name = dto.Name,
        };
    }
    public static BlogEntity MappingBlogEntity(BlogDTO dto)
    {
        return new BlogEntity
        {
            Title = dto.Title,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            UserId = dto.userId,
        };
    }
    public static ContactFormEntity MappingContactForm(ContactDTO dto)
    {
        return new ContactFormEntity
        {
            Name = dto.Name,
            Email = dto.Email,
            Message = dto.Message
        };
    }
    public static CategoryEntity MappingCategory(CategoryDTO dto)
    {
        return new CategoryEntity
        {
            Color = dto.Color,
            IconCssClass = dto.IconCssClass,
            Name = dto.Name,
            imageUrl = dto.ImageUrl
        };
    }
    public static ProductEntity MappingProduct(ProductDTO dto)
    {
        return new ProductEntity
        {
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            DiscountId = dto.DiscountId,
            Price = dto.Price,
            Name = dto.Name,
            StockAmount = dto.StockAmount,
            Enabled = dto.Enabled,
        };
    }

    public static ProductImageEntity MappingProductImage(ProductImageDTO dto)
    {
        return new ProductImageEntity
        {
            Url = dto.Url,
            ProductId = dto.ProductId
        };
    }
}
