using App.Data.Entities;
using SimoshStore;

namespace SimoshStoreAPI;

public interface IOrderService
{
    Task<IServiceResult> CheckOut(OrderEntity order);
    Task<OrderEntity> CheckOrderById(int orderId, int userId);
    Task<OrderEntity> GetOrderByCodeAsync(string orderCode);
    Task<IServiceResult> CreateOrderAsync(OrderDTO dto);
    Task<IEnumerable<OrderEntity>> GetUsersOrdersAsync(int userId);
    int GetOrderCount();
    Task<IEnumerable<OrderEntity>> GetOrdersAsync();
    Task<IServiceResult> DeleteOrderAsync(int id);
    Task<IServiceResult> UpdateOrderAsync(OrderDTO order, int id);
    Task<OrderEntity> GetOrderByIdAsync(int id);
}
