using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Domain.Entities;

namespace ScentSphere.Tests.Integration.Controllers;

[TestFixture]
public class BrandsIntegrationTests
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
    public async Task GetBrands_ReturnsOk()
    {
        // ensure brands seeded by Program or seed here
        var client = _factory!.CreateClient();

        var response = await client.GetAsync("/api/brands");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.IsNotNull(json);
    }
}
