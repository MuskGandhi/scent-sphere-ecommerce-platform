using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Api.Controllers;

namespace ScentSphere.Tests.Unit.Controllers;

[TestFixture]
public class CategoriesControllerTests
{
    private ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var context = new ApplicationDbContext(options);
        return context;
    }

    [Test]
    public async Task GetCategories_ReturnsSeededCategories()
    {
        // arrange
        var context = CreateContext("testdb_categories");
        context.Categories.AddRange(
            new Domain.Entities.Category { Id = Guid.NewGuid(), Name = "Men" },
            new Domain.Entities.Category { Id = Guid.NewGuid(), Name = "Women" }
        );
        await context.SaveChangesAsync();

        var controller = new CategoriesController(context);

        // act
        var result = await controller.GetCategories();

        // assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.IsNotNull(ok.Value);
        var list = ok.Value as System.Collections.IEnumerable;
        Assert.IsNotNull(list);
    }
}
