using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScentSphere.Application.DTOs.Orders;
using ScentSphere.Application.Interfaces;
using ScentSphere.Domain.Entities;
using ScentSphere.Domain.Enums;
using ScentSphere.Persistence.Context;

namespace ScentSphere.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutResponse>
            CheckoutAsync(Guid userId)
        {
            var cart =
                await _context.ShoppingCarts
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

            if (cart == null || !cart.Items.Any())
            {
                return new CheckoutResponse
                {
                    IsSuccess = false,
                    Message = "Cart is empty"
                };
            }

            foreach (var item in cart.Items)
            {
                if (item.Product.StockQuantity <
                    item.Quantity)
                {
                    return new CheckoutResponse
                    {
                        IsSuccess = false,
                        Message =
                            $"{item.Product.Name} is out of stock"
                    };
                }
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalAmount = cart.Items.Sum(
                    x => x.Quantity * x.Product.Price),
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);

            foreach (var item in cart.Items)
            {
                _context.OrderItems.Add(
                    new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice =
                            item.Product.Price
                    });

                item.Product.StockQuantity -=
                    item.Quantity;
            }

            _context.ShoppingCartItems.RemoveRange(
                cart.Items);

            await _context.SaveChangesAsync();

            return new CheckoutResponse
            {
                IsSuccess = true,
                Message =
                    "Order placed successfully",
                OrderId = order.Id
            };
        }

        public async Task<List<OrderDto>>
            GetOrdersAsync(Guid userId)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new OrderDto
                {
                    OrderId = x.Id,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    Items = x.Items.Select(i =>
                        new OrderItemDto
                        {
                            ProductName =
                                i.Product.Name,
                            Quantity =
                                i.Quantity,
                            UnitPrice =
                                i.UnitPrice
                        }).ToList()
                })
                .ToListAsync();
        }

        public async Task<OrderDto?>
            GetOrderByIdAsync(
                Guid userId,
                Guid orderId)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Where(x =>
                    x.UserId == userId &&
                    x.Id == orderId)
                .Select(x => new OrderDto
                {
                    OrderId = x.Id,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    Items = x.Items.Select(i =>
                        new OrderItemDto
                        {
                            ProductName =
                                i.Product.Name,
                            Quantity =
                                i.Quantity,
                            UnitPrice =
                                i.UnitPrice
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
