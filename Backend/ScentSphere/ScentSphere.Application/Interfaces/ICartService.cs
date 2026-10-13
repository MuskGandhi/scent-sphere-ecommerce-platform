using ScentSphere.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse> GetCartAsync(
            Guid userId);

        Task AddToCartAsync(
            Guid userId,
            AddToCartRequest request);

        Task UpdateCartItemAsync(
            Guid cartItemId,
            int quantity);

        Task RemoveCartItemAsync(
            Guid cartItemId);
    }
}
