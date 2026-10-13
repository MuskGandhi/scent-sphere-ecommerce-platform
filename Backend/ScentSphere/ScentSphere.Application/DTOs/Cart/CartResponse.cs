using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Cart
{
    public class CartResponse
    {
        public List<CartItemDto> Items { get; set; }
            = new();

        public decimal CartTotal
        {
            get
            {
                return Items.Sum(
                    x => x.TotalPrice);
            }
        }
    }
}
