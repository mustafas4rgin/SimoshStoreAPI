using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;
using SimoshStoreAPI;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("/api/order-confirmation/{orderId}")]
        public async Task<IActionResult> OrderConfirmAsync(int orderId,[FromBody]int userId)
        {
            var order = await _orderService.CheckOrderById(orderId, userId);
            if(order.Id == 0)
            {
                return NotFound();
            }

            return Ok(order);
        }
        
        [HttpPost("/api/create/order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO dto)
        {
            try
            {
                var result = await _orderService.CreateOrderAsync(dto);
                if(!result.Success)
                {
                    return BadRequest(result.Message);
                }
                var order = await _orderService.GetOrderByCodeAsync(dto.OrderCode);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("/api/order-list/{id}")]
        public async Task<IActionResult> GetOrderList(int id)
        {
            try
            {
                var orders = await _orderService.GetUsersOrdersAsync(id);
                return Ok(new OrderListViewModel
                {
                    orders = orders.ToList()
                });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("/api/orders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("/api/update/order/{id}")]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDTO order,[FromRoute] int id)
        {
            try 
            {
                var result = await _orderService.UpdateOrderAsync(order,id);
                if(!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("/api/delete/order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try 
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if(!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("/api/orders/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try 
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
    }
}
