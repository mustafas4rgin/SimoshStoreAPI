using App.Data.Entities;

namespace SimoshStoreAPI;

public class CheckOutViewModel
{
    public UserEntity User { get; set; } = null!;
    public List<CartItemEntity> cartItems { get; set; } = null!;
    public string OrderNote { get; set; } = "Blank";
    public int shipPrice { get; set; } = 0;

}
