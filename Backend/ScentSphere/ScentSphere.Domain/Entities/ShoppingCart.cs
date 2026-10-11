using ScentSphere.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Domain.Entities
{
  public class ShoppingCart : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<ShoppingCartItem> Items
        { get; set; } = new List<ShoppingCartItem>();
    }
}
