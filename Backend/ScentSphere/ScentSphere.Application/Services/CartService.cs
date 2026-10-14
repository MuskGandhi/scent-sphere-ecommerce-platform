using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScentSphere.Application.DTOs.Cart;
using ScentSphere.Application.Interfaces;
using ScentSphere.Domain.Entities;
using ScentSphere.Persistence.Context;

namespace ScentSphere.Application.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CartService> _logger;

    public CartService(
        ApplicationDbContext context,
        ILogger<CartService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CartResponse>
        GetCartAsync(Guid userId)
    {
        var cart = await _context.ShoppingCarts
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(
                x => x.UserId == userId);

        if (cart == null)
        {
            return new CartResponse();
        }

        return new CartResponse
        {
            Items = cart.Items.Select(x =>
                new CartItemDto
                {
                    CartItemId = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    UnitPrice = x.Product.Price,
                    Quantity = x.Quantity
                }).ToList()
        };
    }

    public async Task AddToCartAsync(Guid userId,
        AddToCartRequest request)
    {
        if (userId == Guid.Empty)
        {
            _logger.LogWarning("AddToCartAsync called with empty userId");
            throw new ArgumentException("userId must be provided", nameof(userId));
        }

        if (request is null)
        {
            _logger.LogWarning("AddToCartAsync called with null request for user {UserId}", userId);
            throw new ArgumentNullException(nameof(request));
        }

        _logger.LogDebug("Adding product {ProductId} (qty {Quantity}) to cart for user {UserId}", request.ProductId, request.Quantity, userId);

        try
        {
            var cart =
                await _context.ShoppingCarts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId
                };

                _context.ShoppingCarts.Add(cart);
                _logger.LogInformation("Created new cart {CartId} for user {UserId}", cart.Id, userId);
            }

            var existingItem =
                cart.Items.FirstOrDefault(
                    x => x.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                _logger.LogDebug("Updated quantity for cart item {CartItemId} to {Quantity}", existingItem.Id, existingItem.Quantity);
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };

                _context.ShoppingCartItems.Add(newItem);
                _logger.LogDebug("Added new cart item {CartItemId} (product {ProductId}) to cart {CartId}", newItem.Id, newItem.ProductId, cart.Id);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("AddToCartAsync completed for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding product {request?.ProductId} to cart for user {userId}", ex);
        }
    }

    public async Task UpdateCartItemAsync(
        Guid cartItemId,
        int quantity)
    {
        if (cartItemId == Guid.Empty)
        {
            _logger.LogWarning("UpdateCartItemAsync called with empty cartItemId");
            throw new ArgumentException("cartItemId must be provided", nameof(cartItemId));
        }

        _logger.LogDebug("Updating cart item {CartItemId} to quantity {Quantity}", cartItemId, quantity);

        try
        {
            var item =
                await _context.ShoppingCartItems
                .FirstOrDefaultAsync(
                    x => x.Id == cartItemId);

            if (item == null)
            {
                _logger.LogInformation("Cart item {CartItemId} not found for update", cartItemId);
                return;
            }

            item.Quantity = quantity;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated cart item {CartItemId} to quantity {Quantity}", cartItemId, quantity);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating cart item {cartItemId}", ex);
        }
    }

    public async Task RemoveCartItemAsync(
        Guid cartItemId)
    {
        if (cartItemId == Guid.Empty)
        {
            _logger.LogWarning("RemoveCartItemAsync called with empty cartItemId");
            throw new ArgumentException("cartItemId must be provided", nameof(cartItemId));
        }

        _logger.LogDebug("Removing cart item {CartItemId}", cartItemId);

        try
        {
            var item =
                await _context.ShoppingCartItems
                .FirstOrDefaultAsync(
                    x => x.Id == cartItemId);

            if (item == null)
            {
                _logger.LogInformation("Cart item {CartItemId} not found for removal", cartItemId);
                return;
            }

            _context.ShoppingCartItems.Remove(item);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Removed cart item {CartItemId}", cartItemId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error removing cart item {cartItemId}", ex);
        }
    }
}