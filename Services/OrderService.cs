using App.Data.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class OrderService : IOrderService
{
    private readonly IDataRepository _Repository;
    private readonly IEmailService _emailService;
    private readonly IValidator<OrderDTO> _Validator;
    public OrderService(IEmailService emailService, IValidator<OrderDTO> Validator, IDataRepository repository)
    {
        _emailService = emailService;
        _Validator = Validator;
        _Repository = repository;
    }
    public async Task<IServiceResult> CheckOut(OrderEntity order)
    {
var cartItemsDetails = string.Join(Environment.NewLine, order.OrderItems.Select(item => 
    $"- Product: {item.Product.Name}, Quantity: {item.Quantity}, Price: ${item.Product.Price:F2}, Total: ${item.Quantity * item.Product.Price:F2}"
));

var totalPrice = order.OrderItems.Sum(item => item.Quantity * item.Product.Price);

await _emailService.SendEmailAsync(
    order.User.Email, 
    "New Order Confirmed", 
    $@"
Hello,

Your new order has been successfully confirmed. Here are the details of your order:

Order Note: {order.Address}

Customer Information:
- Full Name: {order.User.FirstName} {order.User.LastName}
- Address: {order.User.Address}
- Email: {order.User.Email}
- Phone: {order.User.Phone}

Cart Items:
{cartItemsDetails}

Total Price: ${totalPrice:F2}

If you have any questions regarding your order, feel free to contact us.

Wishing you a great day!

Best regards,
Simosh Store
"
);
return new ServiceResult(true,"Email sent successfully.");

    }
    public int GetOrderCount()
    {
        return _Repository.GetAll<OrderEntity>().Count();
    }
    public async Task<IEnumerable<OrderEntity>> GetUsersOrdersAsync(int userId)
    {
        var orders = await _Repository.GetAll<OrderEntity>()
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                           .Where(o => o.UserId == userId)
                           .ToListAsync();
        if (orders == null)
        {
            return new List<OrderEntity>();
        }
        return orders;
    }
    public async Task<OrderEntity> GetOrderByCodeAsync(string orderCode)
    {
        var order = await _Repository.GetAll<OrderEntity>()
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
        if (order is null)
        {
            return new OrderEntity();
        }
        return order;
    }
    public async Task<IServiceResult> CreateOrderAsync(OrderDTO dto)
    {
        var validationResult = _Validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            return new ServiceResult(false, validationResult.Errors.First().ErrorMessage);
        }
        var order = MappingHelper.MappingOrderEntity(dto);
        var user = await _Repository.GetByIdAsync<UserEntity>(dto.UserId);
        if (user is null)
        {
            return new ServiceResult(false, "User not found");
        }
        order.Address = user.Address;
        await _Repository.AddAsync(order);
        var cartItems = await _Repository.GetAll<CartItemEntity>().Where(c => c.UserId == dto.UserId).ToListAsync();
        foreach (var item in cartItems)
        {
            var orderItem = new OrderItemEntity
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
            await _Repository.AddAsync(orderItem);
            await _Repository.DeleteAsync<CartItemEntity>(item.Id);
        }
        return new ServiceResult(true, "Order created successfully");
    }
    public async Task<IServiceResult> UpdateOrderAsync(OrderDTO dto, int id)
    {
        var order = await _Repository.GetByIdAsync<OrderEntity>(id);
        if (order is null)
        {
            return new ServiceResult(false, "Order not found");
        }
        var validationResult = _Validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            return new ServiceResult(false, validationResult.Errors.First().ErrorMessage);
        }
        order.UserId = dto.UserId;
        order.OrderCode = dto.OrderCode;
        await _Repository.UpdateAsync(order);
        return new ServiceResult(true, "Order updated successfully");
    }
    public async Task<OrderEntity> CheckOrderById(int orderId, int userId)
    {
        var order = await _Repository.GetAll<OrderEntity>()
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .ThenInclude(o => o.Product)
                            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null)
        {
            return new OrderEntity();
        }
        if(order.UserId != userId)
        {
            return new OrderEntity();
        }
        var result = await CheckOut(order);

        return order;
    }
    public async Task<OrderEntity> GetOrderByIdAsync(int id)
    {
        var order = await _Repository.GetAll<OrderEntity>()
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .ThenInclude(o => o.Product)
                            .FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
        {
            return new OrderEntity();
        }
        return order;
    }
    public async Task<IEnumerable<OrderEntity>> GetOrdersAsync()
    {
        var orders = await _Repository.GetAll<OrderEntity>()
                           .Include(o => o.User)
                           .Include(o => o.OrderItems)
                           .ToListAsync();
        if (orders == null)
        {
            return new List<OrderEntity>();
        }
        return orders;
    }
    public async Task<IServiceResult> DeleteOrderAsync(int id)
    {
        var order = await _Repository.GetByIdAsync<OrderEntity>(id);
        if (order is null)
        {
            return new ServiceResult(false, "Order not found");
        }
        await _Repository.DeleteAsync<OrderEntity>(id);
        return new ServiceResult(true, "Order deleted successfully");
    }
}
