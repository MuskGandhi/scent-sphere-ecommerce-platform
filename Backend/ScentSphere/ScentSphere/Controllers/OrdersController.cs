using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScentSphere.Application.Interfaces;
using System.Security.Claims;

namespace ScentSphere.Api.Controllers
{

    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(
            IOrderService service)
        {
            _service = service;
        }

        private Guid CurrentUserId =>
            Guid.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!
                .Value);

        [HttpPost("checkout")]
        public async Task<IActionResult>
            Checkout()
        {
            var result =
                await _service.CheckoutAsync(
                    CurrentUserId);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult>
            GetOrders()
        {
            return Ok(
                await _service.GetOrdersAsync(
                    CurrentUserId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>
            GetOrder(Guid id)
        {
            var order =
                await _service.GetOrderByIdAsync(
                    CurrentUserId,
                    id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}
