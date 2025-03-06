namespace SimoshStoreAPI;

using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimoshStore;

public class ProductService : IProductService
{
    private readonly IDataRepository _Repository;
    public ProductService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<IServiceResult> UpdateProductAsync(ProductDTO dto, int id)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(id);
        if (product is null)
        {
            return new ServiceResult(false, "Product not found");
        }
        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Description = dto.Description;
        product.StockAmount = dto.StockAmount;
        product.CategoryId = dto.CategoryId;
        product.DiscountId = dto.DiscountId;
        product.SellerId = 2;
        await _Repository.UpdateAsync(product);
        return new ServiceResult(true, "Product updated successfully");
    }
    public async Task<ProductEntity> GetProductByIdAsync(int id)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(id);
        if (product is null)
        {
            throw new Exception("Product not found");
        }
        return product;
    }
    public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
    {
        var products = await _Repository.GetAll<ProductEntity>().
                            Include(P=>P.Category).
                            Include(P=>P.Discount).
                            Include(P=>P.Images).ToListAsync();
        if (products == null)
        {
            throw new Exception("No products found");
        }
        return products;
    }
    public async Task<IServiceResult> DeleteProductAsync(int id)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(id);
        if (product is null)
        {
            return new ServiceResult(false, "Product not found");
        }
        await _Repository.DeleteAsync<ProductEntity>(id);
        return new ServiceResult(true, "Product deleted successfully");
    }
    public async Task<IServiceResult> CreateProductAsync(ProductDTO dto)
    {
        var product = MappingHelper.MappingProduct(dto);
        product.SellerId = 2;
        if (product is null)
        {
            return new ServiceResult(false, "Product is null");
        }
        if (product.Name is null)
        {
            return new ServiceResult(false, "Product name is null");
        }
        if (product.Price <= 0)
        {
            return new ServiceResult(false, "Product price is invalid");
        }
        if (product.Description is null)
        {
            return new ServiceResult(false, "Product description is null");
        }
        if (product.StockAmount <= 0)
        {
            return new ServiceResult(false, "Product stock amount is invalid");
        }
        if(product.CategoryId <= 0)
        {
            return new ServiceResult(false, "Product category is null");
        }
        var category = await _Repository.GetByIdAsync<CategoryEntity>(product.CategoryId);
        if (category is not null)
        {
            product.Category = category;
        }
        var comments = await _Repository.GetAll<ProductCommentEntity>().Where(c => c.ProductId == product.Id).ToListAsync();
        if(comments is not null)
        {
            product.Comments = comments;
        }
        if(product.DiscountId is not null)
        {
            var discount = await _Repository.GetByIdAsync<DiscountEntity>(product.DiscountId.Value);
            if(discount is not null)
            {
                product.Discount = discount;
            }
        }
        var images = _Repository.GetAll<ProductImageEntity>().Where(i => i.ProductId == product.Id).ToList();
        if(images is null)
        {
            await _Repository.AddAsync(
                new ProductImageEntity
                {
                    ProductId = product.Id,
                     Url = "default.jpg"
                }
            );
        };
        await _Repository.AddAsync(product);
        return new ServiceResult(true, "Product created successfully");
    }
}
