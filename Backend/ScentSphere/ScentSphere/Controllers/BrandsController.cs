using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScentSphere.Persistence.Context;

namespace ScentSphere.Api.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public BrandsController(
        ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var brands =
            await _context.Brands
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .ToListAsync();

        return Ok(brands);
    }
}