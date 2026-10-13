using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Orders
{
    public class CheckoutResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public Guid OrderId { get; set; }
    }
}
