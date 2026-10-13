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

        public ShoppingCart ShoppingCart { get; set; } = null!;

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
