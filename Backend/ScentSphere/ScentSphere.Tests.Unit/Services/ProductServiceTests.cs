using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Application.Services;
using ScentSphere.Application.DTOs.Products;
using ScentSphere.Domain.Entities;

namespace ScentSphere.Tests.Unit.Services;

[TestFixture]
public class ProductServiceTests
{
    private ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    [Test]
    public async Task CreateAndGetProduct_Works()
    {
        var ctx = CreateContext("prod_create_db");

        var brand = new Brand { Id = Guid.NewGuid(), Name = "TestBrand" };
        var category = new Category { Id = Guid.NewGuid(), Name = "TestCat" };
        ctx.Brands.Add(brand);
        ctx.Categories.Add(category);
        await ctx.SaveChangesAsync();

        // seed a product (RowVersion must be set for InMemory provider)
        var p = new Product { Id = Guid.NewGuid(), Name = "P1", Description = "Desc", Price = 9.99m, StockQuantity = 10, BrandId = brand.Id, CategoryId = category.Id, RowVersion = new byte[] { 1 } };
        ctx.Products.Add(p);
        await ctx.SaveChangesAsync();

        var svc = new ProductService(ctx);

        var all = await svc.GetAllAsync();
        Assert.IsNotEmpty(all);

        var product = all.First();
        Assert.AreEqual("P1", product.Name);

        var byId = await svc.GetByIdAsync(product.Id);
        Assert.IsNotNull(byId);

        var search = await svc.SearchAsync("P1");
        Assert.IsNotEmpty(search);
    }
}
