using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace ScentSphere.Tests.Integration.Controllers;

[TestFixture]
public class CategoriesIntegrationTests
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
    public async Task GetCategories_ReturnsOk()
    {
        var client = _factory!.CreateClient();

        var response = await client.GetAsync("/api/categories");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.IsNotNull(json);
        // expecting an array
        Assert.IsTrue(json.TrimStart().StartsWith("[") || json.TrimStart().StartsWith("{"));
    }
}
