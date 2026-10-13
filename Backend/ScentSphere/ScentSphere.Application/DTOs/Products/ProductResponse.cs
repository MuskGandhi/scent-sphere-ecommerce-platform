using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Products
{
    public class ProductResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
