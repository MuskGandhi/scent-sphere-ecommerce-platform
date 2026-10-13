using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Application.Services;
using ScentSphere.Domain.Entities;
using ScentSphere.Domain.Enums;

namespace ScentSphere.Tests.Unit.Services;

[TestFixture]
public class OrderServiceTests
{
    private ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    [Test]
    public async Task Checkout_ReducesStockAndCreatesOrder()
    {
        var ctx = CreateContext("order_db");

        var user = new User { Id = Guid.NewGuid(), FirstName = "A", LastName = "B", Email = "u@x.com", PasswordHash = "h", RoleId = Guid.NewGuid(), IsActive = true };
        ctx.Users.Add(user);

        var product = new Product { Id = Guid.NewGuid(), Name = "Prod", Price = 10m, StockQuantity = 5, RowVersion = new byte[] { 1 } };
        ctx.Products.Add(product);

        var cart = new ShoppingCart { Id = Guid.NewGuid(), UserId = user.Id };
        ctx.ShoppingCarts.Add(cart);

        var item = new ShoppingCartItem { Id = Guid.NewGuid(), CartId = cart.Id, ProductId = product.Id, Quantity = 2 };
        ctx.ShoppingCartItems.Add(item);

        await ctx.SaveChangesAsync();

        var svc = new OrderService(ctx);

        var res = await svc.CheckoutAsync(user.Id);
        Assert.IsTrue(res.IsSuccess);

        var ord = ctx.Orders.Include(x => x.Items).FirstOrDefault();
        Assert.IsNotNull(ord);
        Assert.AreEqual(1, ord.Items.Count);

        var prodAfter = await ctx.Products.FindAsync(product.Id);
        Assert.AreEqual(3, prodAfter.StockQuantity);
    }
}
