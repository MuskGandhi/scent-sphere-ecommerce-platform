using ScentSphere.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Domain.Entities
{
    public class Wishlist : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public User User { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
