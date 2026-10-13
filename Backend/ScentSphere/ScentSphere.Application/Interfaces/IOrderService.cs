using ScentSphere.Application.DTOs.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
