using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ScentSphere.Tests.Integration.Controllers;

[TestFixture]
public class ProductsIntegrationTests
{
    private WebApplicationFactory<Program>? _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [TearDown]
    public void TearDown()
    {
        _factory?.Dispose();
    }

    [Test]
    public async Task GetAllProducts_ReturnsOk()
    {
        // seed a product
        using (var scope = _factory!.Services.CreateScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var brand = new Brand { Id = Guid.NewGuid(), Name = "B" };
            var category = new Category { Id = Guid.NewGuid(), Name = "C" };
            ctx.Brands.Add(brand);
            ctx.Categories.Add(category);
            ctx.Products.Add(new Product { Id = Guid.NewGuid(), Name = "Prod1", BrandId = brand.Id, CategoryId = category.Id, Price = 1m, StockQuantity = 1 });
            await ctx.SaveChangesAsync();
        }

        var client = _factory!.CreateClient();

        var response = await client.GetAsync("/api/products");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.IsNotNull(json);
    }
}
