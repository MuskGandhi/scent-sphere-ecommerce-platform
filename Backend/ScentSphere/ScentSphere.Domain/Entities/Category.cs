using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::ScentSphere.Domain.Common;
using ScentSphere.Domain.Entities;

namespace ScentSphere.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}
