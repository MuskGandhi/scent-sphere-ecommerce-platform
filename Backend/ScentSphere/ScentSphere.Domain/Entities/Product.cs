using ScentSphere.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Domain.Entities
{

    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsDeleted { get; set; }

        public Guid BrandId { get; set; }

        public Guid CategoryId { get; set; }

        public Brand Brand { get; set; } = null!;

        public Category Category { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; }
            = new List<ProductImage>();

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
    }
}
