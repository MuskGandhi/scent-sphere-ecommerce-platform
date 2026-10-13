using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScentSphere.Application.Interfaces;
using ScentSphere.Persistence.Context;

namespace ScentSphere.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(
        ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories =
            await _context.Categories
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToListAsync();

        return Ok(categories);
    }
}