using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScentSphere.Application.Interfaces;

namespace ScentSphere.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products =
            await _productService.GetAllAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var product =
            await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string keyword)
    {
        var products =
            await _productService
                .SearchAsync(keyword);

        return Ok(products);
    }
}