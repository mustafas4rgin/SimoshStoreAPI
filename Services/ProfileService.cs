using App.Data.Entities;
using SimoshStore;

namespace SimoshStoreAPI;

public class ProfileService : IProfileService
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly ICommentService _commentService;
    private readonly IDataRepository _dataRepository;
    public ProfileService(IDataRepository dataRepository, IUserService userService, IOrderService orderService, ICommentService commentService)
    {
        _dataRepository = dataRepository;
        _userService = userService;
        _orderService = orderService;
        _commentService = commentService;
    }
    public async Task<IServiceResult> UpdateAddressAsync(int id, EditAddressViewModel model)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if(user is null)
        {
            return new ServiceResult(false, "User not found");
        }
        user.Address = $"{model.FirstName} {model.LastName} {model.Street}, {model.Phone}, {model.Email}";
        await _dataRepository.UpdateAsync(user);
        return new ServiceResult(true, "Address updated successfully");
    }
    public async Task<ProfileViewModel> GetProfileAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        var orders = await _orderService.GetUsersOrdersAsync(userId);
        var productComments = await _commentService.GetProductCommentsByUserId(userId);
        if(user is null)
        {
            return null;
        }
        if(orders is null)
        {
            orders = new List<OrderEntity>();
        }
        if(productComments is null)
        {
            productComments = new List<ProductCommentEntity>();
        }
        decimal totalPrice = 0;
        foreach(var order in orders)
        {
            foreach(var item in order.OrderItems)
            {
                totalPrice += item.UnitPrice * item.Quantity;
            }
        }
        return new ProfileViewModel
        {
            TotalPrice = totalPrice,
            User = user,
            Orders = orders.ToList(),
            ProductComments = productComments.ToList()
        };
    }
}
