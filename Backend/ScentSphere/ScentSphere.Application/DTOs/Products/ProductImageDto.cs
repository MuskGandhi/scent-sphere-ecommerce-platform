using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Products
{
    public class ProductImageDto
    {
        public string ImageUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}
