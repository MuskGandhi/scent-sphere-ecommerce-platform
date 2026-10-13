using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Orders
{
  
    public class OrderItemDto
    {
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice =>
            Quantity * UnitPrice;
    }
}
