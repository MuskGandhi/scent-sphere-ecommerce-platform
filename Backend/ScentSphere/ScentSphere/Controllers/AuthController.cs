using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScentSphere.Application.DTOs.Auth;
using ScentSphere.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ScentSphere.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            var result =
                await _authService.RegisterAsync(request);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result =
                await _authService.LoginAsync(
                    request);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();

            if (!result.IsSuccess)
                return BadRequest(result);
            if (Request?.Cookies != null)
            {
                if (Request.Cookies.ContainsKey("refreshToken"))
                    Response.Cookies.Delete("refreshToken");

                if (Request.Cookies.ContainsKey("accessToken"))
                    Response.Cookies.Delete("accessToken");
            }

            return Ok(result);
        }
    }
}
