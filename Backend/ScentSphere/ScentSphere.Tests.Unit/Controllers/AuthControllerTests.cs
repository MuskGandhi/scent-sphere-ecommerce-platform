using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ScentSphere.Api.Controllers;
using ScentSphere.Application.DTOs.Auth;
using ScentSphere.Application.Interfaces;

namespace ScentSphere.Tests.Unit.Controllers;

public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock = null!;
    private AuthController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [TearDown]
    public void Teardown()
    {
        _authServiceMock.VerifyAll();
    }

    [Test]
    public async Task Register_ReturnsOk_WhenServiceSucceeds()
    {
        var req = new RegisterRequest { Email = "a@b.com", Password = "p" };
        var resp = new AuthResponse { IsSuccess = true, Token = "t" };

        _authServiceMock.Setup(x => x.RegisterAsync(req))
            .ReturnsAsync(resp)
            .Verifiable();

        var result = await _controller.Register(req);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.AreSame(resp, ok.Value);
    }

    [Test]
    public async Task Register_ReturnsBadRequest_WhenServiceFails()
    {
        var req = new RegisterRequest();
        var resp = new AuthResponse { IsSuccess = false, Message = "dup" };

        _authServiceMock.Setup(x => x.RegisterAsync(req))
            .ReturnsAsync(resp)
            .Verifiable();

        var result = await _controller.Register(req);

        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var bad = (BadRequestObjectResult)result;
        Assert.AreSame(resp, bad.Value);
    }

    [Test]
    public async Task Login_ReturnsOk_WhenCredentialsValid()
    {
        var req = new LoginRequest { Email = "a@b.com", Password = "p" };
        var resp = new AuthResponse { IsSuccess = true, Token = "t" };

        _authServiceMock.Setup(x => x.LoginAsync(req))
            .ReturnsAsync(resp)
            .Verifiable();

        var result = await _controller.Login(req);

        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.AreSame(resp, ok.Value);
    }

    [Test]
    public async Task Login_ReturnsBadRequest_WhenInvalidCredentials()
    {
        var req = new LoginRequest { Email = "a@b.com", Password = "wrong" };
        var resp = new AuthResponse { IsSuccess = false, Message = "invalid" };

        _authServiceMock.Setup(x => x.LoginAsync(req))
            .ReturnsAsync(resp)
            .Verifiable();

        var result = await _controller.Login(req);

        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var bad = (BadRequestObjectResult)result;
        Assert.AreSame(resp, bad.Value);
    }

    [Test]
    public async Task Logout_DeletesCookiesAndReturnsOk_WhenServiceSucceeds()
    {
        var resp = new AuthResponse { IsSuccess = true };
        _authServiceMock.Setup(x => x.LogoutAsync())
            .ReturnsAsync(resp)
            .Verifiable();

        // add cookies to request so controller will attempt delete
        _controller.ControllerContext.HttpContext.Request.Headers["Cookie"] = "refreshToken=1; accessToken=2";

        var result = await _controller.Logout();

        Assert.IsInstanceOf<OkObjectResult>(result);
        var ok = (OkObjectResult)result;
        Assert.AreSame(resp, ok.Value);
    }

    [Test]
    public async Task Logout_ReturnsBadRequest_WhenServiceFails()
    {
        var resp = new AuthResponse { IsSuccess = false, Message = "err" };
        _authServiceMock.Setup(x => x.LogoutAsync())
            .ReturnsAsync(resp)
            .Verifiable();

        var result = await _controller.Logout();

        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var bad = (BadRequestObjectResult)result;
        Assert.AreSame(resp, bad.Value);
    }
}
