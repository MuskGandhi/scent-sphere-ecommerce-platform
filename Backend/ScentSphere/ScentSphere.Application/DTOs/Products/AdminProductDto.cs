using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Products
{
    public class AdminProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public bool IsFeatured { get; set; }

        public Guid BrandId { get; set; }

        public Guid CategoryId { get; set; }
    }
}
