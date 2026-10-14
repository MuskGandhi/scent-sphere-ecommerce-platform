using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ScentSphere.Api.Controllers;
using ScentSphere.Application.DTOs.Products;
using ScentSphere.Application.Interfaces;

namespace ScentSphere.Tests.Unit.Controllers;

public class ProductsControllerTests
{
    private Mock<IProductService> _productServiceMock = null!;
    private ProductsController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductsController(_productServiceMock.Object);
    }

    [TearDown]
    public void Teardown()
    {
        _productServiceMock.VerifyAll();
    }

    [Test]
    public async Task GetAll_ReturnsOk_WithProducts()
    {
        var list = new List<ProductDto> { new ProductDto { Id = Guid.NewGuid(), Name = "p" } };
        _productServiceMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(list)
            .Verifiable();

        var result = await _controller.GetAll();

        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.AreSame(list, ok.Value);
    }

    [Test]
    public async Task GetById_ReturnsOk_WhenFound()
    {
        var id = Guid.NewGuid();
        var dto = new ProductDto { Id = id, Name = "x" };
        _productServiceMock.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(dto)
            .Verifiable();

        var result = await _controller.GetById(id);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.AreSame(dto, ok.Value);
    }

    [Test]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        var id = Guid.NewGuid();
        _productServiceMock.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync((ProductDto?)null)
            .Verifiable();

        var result = await _controller.GetById(id);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Search_ReturnsOk_WithResults()
    {
        var keyword = "scent";
        var list = new List<ProductDto>();
        _productServiceMock.Setup(x => x.SearchAsync(keyword))
            .ReturnsAsync(list)
            .Verifiable();

        var result = await _controller.Search(keyword);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.AreSame(list, ok.Value);
    }
}
