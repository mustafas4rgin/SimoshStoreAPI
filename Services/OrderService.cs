using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class OrderService : IOrderService
{
    private readonly IDataRepository _Repository;
    public OrderService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<IServiceResult> UpdateOrderAsync(OrderDTO dto, int id)
    {
        var order = await _Repository.GetByIdAsync<OrderEntity>(id);
        if (order is null)
        {
            return new ServiceResult(false, "Order not found");
        }
        order.User = await _Repository.GetAll<UserEntity>().FirstOrDefaultAsync(x => x.Id == dto.UserId);
        if (order.User is null)
        {
            return new ServiceResult(false, "User not found");
        }
        order.OrderItems = await _Repository.GetAll<OrderItemEntity>().Where(x => x.OrderId == id).ToListAsync();
        if (order.OrderItems is null)
        {
            return new ServiceResult(false, "Order items not found");
        }
        order.Address = dto.Address;
        order.UserId = dto.UserId;
        order.OrderCode = dto.OrderCode;
        await _Repository.UpdateAsync(order);
        return new ServiceResult(true, "Order updated successfully");
    }
    public async Task<OrderEntity> GetOrderByIdAsync(int id)
    {
        var order = await _Repository.GetAll<OrderEntity>()
                            .Include(o => o.User)
                            .Include(o => o.OrderItems)
                            .FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
        {
            throw new Exception("Order not found");
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
            throw new Exception("No orders found");
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
