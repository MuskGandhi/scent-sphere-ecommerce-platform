using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScentSphere.Application.DTOs.Products;
using ScentSphere.Application.Interfaces;

namespace ScentSphere.Api.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Admin")]
public class AdminProductsController
    : ControllerBase
{
    private readonly IProductService _productService;

    public AdminProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult>GetProducts()
    {
        var products =
            await _productService
                .GetAdminProductsAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>GetProduct(Guid id)
    {
        var product =
            await _productService
                .GetAdminProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateProductRequest request)
    {
        var result =
            await _productService
                .CreateAsync(request);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateProductRequest request)
    {
        var result =
            await _productService
                .UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        Guid id)
    {
        var result =
            await _productService
                .DeleteAsync(id);

        return Ok(result);
    }
}