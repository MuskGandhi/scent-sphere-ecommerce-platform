using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ScentSphere.Persistence.Context;
using ScentSphere.Application.Services;
using ScentSphere.Application.DTOs.Auth;
using ScentSphere.Application.Interfaces;
using ScentSphere.Domain.Entities;

namespace ScentSphere.Tests.Unit.Services;

[TestFixture]
public class AuthServiceTests
{
    private ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    [Test]
    public async Task RegisterAndLogin_Works()
    {
        var ctx = CreateContext("auth_db");

        var passwordHasher = new Mock<IPasswordHasher>();
        passwordHasher.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed");
        passwordHasher.Setup(x => x.VerifyPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var jwt = new Mock<IJwtTokenService>();
        jwt.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("tok");

        var regReq = new RegisterRequest
        {
            FirstName = "F",
            LastName = "L",
            Email = "t@example.com",
            Password = "pwd",
            PhoneNumber = "123"
        };

        var loginReq = new LoginRequest { Email = "t@example.com", Password = "pwd" };
        
    }
}
