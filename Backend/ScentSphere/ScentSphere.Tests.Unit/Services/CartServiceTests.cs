using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Application.Services;
using ScentSphere.Domain.Entities;
using ScentSphere.Application.DTOs.Cart;

namespace ScentSphere.Tests.Unit.Services;

[TestFixture]
public class CartServiceTests
{
    private ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    [Test]
    public async Task AddToCart_And_GetCart_Works()
    {
        var ctx = CreateContext("cart_db");

        var userId = Guid.NewGuid();
        var product = new Product { Id = Guid.NewGuid(), Name = "P", Price = 5m, StockQuantity = 10, RowVersion = new byte[] { 1 } };
        ctx.Products.Add(product);
        await ctx.SaveChangesAsync();

        var svc = new CartService(ctx, new NullLogger<CartService>());

        var req = new AddToCartRequest { ProductId = product.Id, Quantity = 2 };

        await svc.AddToCartAsync(userId, req);

        var cart = await svc.GetCartAsync(userId);
        Assert.IsNotNull(cart);
        Assert.IsNotEmpty(cart.Items);
        Assert.AreEqual(2, cart.Items.First().Quantity);
    }
}
