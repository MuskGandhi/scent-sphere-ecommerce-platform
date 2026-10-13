using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScentSphere.Application.DTOs.Cart;
using ScentSphere.Application.Interfaces;

namespace ScentSphere.Api.Controllers
{

    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(
            ICartService service)
        {
            _service = service;
        }

        private Guid CurrentUserId =>
            Guid.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!
                .Value);

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(
                await _service.GetCartAsync(
                    CurrentUserId));
        }

        [HttpPost("items")]
        public async Task<IActionResult> Add(
            AddToCartRequest request)
        {
            await _service.AddToCartAsync(
                CurrentUserId,
                request);

            return Ok();
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateCartItemRequest request)
        {
            await _service.UpdateCartItemAsync(
                id,
                request.Quantity);

            return Ok();
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            await _service.RemoveCartItemAsync(id);

            return Ok();
        }
    }
}