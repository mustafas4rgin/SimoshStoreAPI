using App.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimoshStore;

namespace SimoshStoreAPI;

public class CartService : ICartService
{
    private readonly IDataRepository _Repository;
    private readonly IEmailService _emailService;
    public CartService(IEmailService emailService, IDataRepository repository)
    {
        _emailService = emailService;
        _Repository = repository;
    }
    
    public async Task<CheckOutViewModel> CheckOut(int userId)
    {
        var user = await _Repository.GetByIdAsync<UserEntity>(userId);

        var cartItems = await _Repository.GetAll<CartItemEntity>()
                        .Include(c => c.Product)
                        .ThenInclude(p => p.Discount)
                        .ToListAsync();
        if (user == null || cartItems == null)
        {
            return new CheckOutViewModel();
        }

        return new CheckOutViewModel
        {
            cartItems = cartItems,
            User = user,
        };

    }
    public async Task<IServiceResult> ClearCartAsync(int userId)
    {
        var cartItems = await _Repository.GetAll<CartItemEntity>().Where(x => x.UserId == userId).ToListAsync();
        if (cartItems.Count == 0)
        {
            return new ServiceResult(false, "There is no item.");
        }
        foreach (var cartItem in cartItems)
        {
            await _Repository.DeleteAsync<CartItemEntity>(cartItem.Id);
        }
        return new ServiceResult(true, "Cart cleared successfully.");
    }
    public async Task<IServiceResult> DeleteCartItem(int userId, int productId)
    {
        var cartItem = await _Repository.GetAll<CartItemEntity>().Where(C => C.ProductId == productId && C.UserId == userId).FirstOrDefaultAsync();
        if (cartItem is null)
        {
            return new ServiceResult(false, "There is no item.");
        }
        await _Repository.DeleteAsync<CartItemEntity>(cartItem.Id);
        return new ServiceResult(true, "Item deleted successfully.");
    }
    public async Task<IEnumerable<CartItemEntity>> GetCartItems(int userId)
    {
        var cartItems = await _Repository.GetAll<CartItemEntity>()
                                .Include(C => C.Product)
                                .ThenInclude(P => P.Discount)
                                .Include(C => C.User)
                                .Where(c => c.UserId == userId).ToListAsync();
        if (cartItems is null)
        {
            return new List<CartItemEntity>();
        }
        return cartItems;
    }
    public async Task<IServiceResult> AddToCart(int userId, CartDTO dto)
    {
        var product = await _Repository.GetByIdAsync<ProductEntity>(dto.ProductId);

        if (product == null)
        {
            return new ServiceResult(false, "There is no product");
        }

        var existingCartItem = await _Repository.GetAll<CartItemEntity>()
            .FirstOrDefaultAsync(C => C.ProductId == dto.ProductId && C.UserId == userId);

        if (existingCartItem is not null)
        {
            existingCartItem.Quantity += dto.Quantity;
            await _Repository.UpdateAsync(existingCartItem);
        }
        else
        {
            var cartItem = new CartItemEntity
            {
                Quantity = dto.Quantity,
                ProductId = dto.ProductId,
                UserId = userId,
            };
            await _Repository.AddAsync(cartItem);
        }
        return new ServiceResult(true, "Cart item added successfully.");
    }
}
