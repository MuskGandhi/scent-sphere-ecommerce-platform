using ScentSphere.Domain.Enums;

namespace ScentSphere.Application.DTOs.Orders
{
    
    public class OrderDto
    {
        public Guid OrderId { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemDto> Items { get; set; }
            = new();
    }
}
