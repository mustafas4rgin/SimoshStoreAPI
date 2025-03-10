namespace SimoshStoreAPI;

using App.Data.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimoshStore;

public class ProductService : IProductService
{
    private readonly IDataRepository _Repository;
    private readonly IValidator<ProductDTO> _Validator;
    public ProductService(ICategoryService categoryService, IValidator<ProductDTO> validator, IDataRepository repository)
    {
        _Validator = validator;
        _Repository = repository;
    }
    public async Task<SearchBarViewModel> SearchProduct()
    {
        var popularProducts = await _Repository.GetAll<ProductEntity>()
                            .Include(P => P.Discount).OrderBy(P => P.Comments.Count()).ToListAsync();

        var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();

        if (popularProducts.Count() <= 0 || categories.Count() <= 0)
        {
            return new SearchBarViewModel();
        }

        return new SearchBarViewModel
        {
            Categories = categories,
            Products = popularProducts,
        };
    }
    public async Task<UpdateProductDTO> AdminUpdate(int id)
    {
        var product = await _Repository.GetAll<ProductEntity>()
                            .Include(P => P.Discount)
                            //.Include(P => P.Images)
                            .FirstOrDefaultAsync(P => P.Id == id);

        if (product == null)
        {
            return new UpdateProductDTO();
        }

        var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();

        var discounts = await _Repository.GetAll<DiscountEntity>().ToListAsync();

        return new UpdateProductDTO
        {
            Categories = categories,
            Discounts = discounts,
            Product = product
        };
    }
    public async Task<IEnumerable<ProductEntity>> PopularProductsAsync(int? take)
    {
        var popularProducts = await _Repository.GetAll<ProductEntity>()
                                .OrderBy(P => P.Comments.Count)
                                .Include(P => P.Discount)
                                .Include(P => P.Comments)
                                .ToListAsync();

        if (take.HasValue)
        {
            popularProducts = popularProducts.Take(take.Value).ToList();
        }

        return popularProducts;
    }
    public async Task<IServiceResult> UpdateProductAsync(ProductDTO dto, int id)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(id);
        if (product is null)
        {
            return new ServiceResult(false, "Product not found");
        }
        var validationResult = _Validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            return new ServiceResult(false, validationResult.Errors.First().ErrorMessage);
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
    public async Task<ProductDetailsViewModel> GetProductByIdAsync(int id)
    {
        var product = await _Repository.GetAll<ProductEntity>()
                            .Include(p => p.Category)
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.User)
                            //.Include(p => p.Images)
                            .FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            throw new Exception("Product not found");
        }
        int productStar = 0;
        if (product.Comments.Count > 0)
        {
            productStar = product.Comments.Sum(c => c.StarCount) / product.Comments.Count;
        }
        var similarProducts = await _Repository.GetAll<ProductEntity>()
                            .Include(p => p.Category)
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.User)
                            .Include(p => p.Discount)
                            //.Include(p => p.Images)
                            .Where(p => p.CategoryId == product.CategoryId)
                            .ToListAsync();
        if (similarProducts.Count <= 0)
        {
            throw new Exception("No similar products found");
        }

        var relatedProducts = await _Repository.GetAll<ProductEntity>()
                            .Include(p => p.Category)
                            .Include(p => p.Comments)
                            .ThenInclude(c => c.User)
                            .Include(p => p.Discount)
                            //.Include(p => p.Images)
                            .Where(p => p.CategoryId != product.CategoryId)
                            .ToListAsync();
        if (relatedProducts.Count <= 0)
        {
            throw new Exception("No related products found");
        }

        return new ProductDetailsViewModel
        {
            Star = productStar,
            Product = product,
            SimilarProducts = similarProducts,
            RelatedProducts = relatedProducts
        };
    }
    public async Task<IEnumerable<ProductEntity>> GetProductsAsync()
    {
        var products = await _Repository.GetAll<ProductEntity>()
            .Include(p => p.Discount)
            .Include(p => p.Category)
            .Include(p => p.Comments)
            .ThenInclude(c => c.User)
            .ToListAsync();


        if (!products.Any())
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
        var validationResult = _Validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            return new ServiceResult(false, validationResult.Errors.First().ErrorMessage);
        }
        var product = MappingHelper.MappingProduct(dto);

        var images = _Repository.GetAll<ProductImageEntity>().Where(i => i.ProductId == product.Id).ToList();
        if (images is null)
        {
            await _Repository.AddAsync(
                new ProductImageEntity
                {
                    ProductId = product.Id,
                    Url = "default.jpg"
                }
            );
        }
        ;
        await _Repository.AddAsync(product);
        return new ServiceResult(true, "Product created successfully");
    }
    public async Task<IEnumerable<ProductEntity>> BestProductsAsync()
    {
        var products = await _Repository.GetAll<ProductEntity>()
            .Include(p => p.Category)
            .Include(p => p.Discount)
        //  .Include(p => p.Images)
            .OrderByDescending(p => p.StockAmount)
            .Take(7)
            .ToListAsync();
        if (!products.Any())
        {
            throw new Exception("No products found");
        }
        return products;
    }
}
