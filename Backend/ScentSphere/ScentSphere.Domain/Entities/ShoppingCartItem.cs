using ScentSphere.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Domain.Entities
{
    public class ShoppingCartItem : BaseEntity
    {
        public Guid CartId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public ShoppingCart Cart { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
