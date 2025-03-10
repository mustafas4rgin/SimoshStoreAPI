using App.Data.Entities;
using SimoshStore;

namespace SimoshStoreAPI;

public interface ICartService
{
    
    Task<IServiceResult> DeleteCartItem(int userId, int productId);
    Task<IEnumerable<CartItemEntity>> GetCartItems(int userId);
    Task<IServiceResult> AddToCart(int id, CartDTO dto);
    Task<IServiceResult> ClearCartAsync(int userId);
    Task<CheckOutViewModel> CheckOut(int userId);
}
