using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScentSphere.Application.DTOs.Auth;
using ScentSphere.Application.Interfaces;
using ScentSphere.Domain.Entities;
using ScentSphere.Domain.Enums;
using ScentSphere.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ScentSphere.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthService(
            ApplicationDbContext context,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IJwtTokenService jwtTokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser =
                    await _context.Users
                    .FirstOrDefaultAsync(x =>
                        x.Email == request.Email);

                if (existingUser != null)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Email already exists"
                    };
                }

                var customerRole =
                    await _context.Roles
                    .FirstOrDefaultAsync(x =>
                        x.Name == "Customer");

                if (customerRole == null)
                {
                    customerRole = new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Customer",
                        Description = "Customer",
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Roles.Add(customerRole);

                    await _context.SaveChangesAsync();
                }

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = _passwordHasher.HashPassword(
                        request.Password),
                    RoleId = customerRole.Id,
                    IsEmailVerified = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                await _context.SaveChangesAsync();

                //await _emailService.SendEmailAsync(
                //    user.Email,
                //    "Welcome to ScentSphere",
                //    $"""
                //        Hi {user.FirstName},

                //        Welcome to ScentSphere.

                //        Your account has been successfully created.

                //        You can now login and start exploring our perfume collection.

                //        Happy Shopping!

                //        Team ScentSphere
                //        """);

                return new AuthResponse
                {
                    IsSuccess = true,
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user =
                await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(
                    x => x.Email == request.Email);

            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            var passwordValid =
                _passwordHasher.VerifyPassword(
                    request.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            }

            var token =
                _jwtTokenService.GenerateToken(user);

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                Token = token,
                Role = user.Role.Name,
                UserName = $"{user.FirstName} {user.LastName}"
            };
        }

        public async Task<AuthResponse> VerifyEmailAsync(
            VerifyEmailRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> LogoutAsync()
        {
            return await Task.FromResult(new AuthResponse
            {
                IsSuccess = true,
                Message = "Logout successful"
            });
        }

        private string GenerateOtp()
        {
            return Random.Shared
                .Next(100000, 999999)
                .ToString();
        }
    }
}
