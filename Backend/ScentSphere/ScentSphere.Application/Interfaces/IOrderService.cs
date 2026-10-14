using ScentSphere.Application.DTOs.Orders;

namespace ScentSphere.Application.Interfaces
{
    public interface IOrderService
    {
        Task<CheckoutResponse> CheckoutAsync(
            Guid userId);

        Task<List<OrderDto>> GetOrdersAsync(
            Guid userId);

        Task<OrderDto?> GetOrderByIdAsync(
            Guid userId,
            Guid orderId);
    }
}
