using ScentSphere.Domain.Common;
using ScentSphere.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public User User { get; set; } = null!;

        public ICollection<OrderItem> Items
        { get; set; } = new List<OrderItem>();
    }
}
