using ScentSphere.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Infrastructure.Email
{
    public class MockEmailService : IEmailService
    {
        public Task SendEmailAsync(
            string to,
            string subject,
            string body)
        {
            Console.WriteLine(
                $"Email To:{to}");

            Console.WriteLine(
                $"Subject:{subject}");

            Console.WriteLine(
                $"Body:{body}");

            return Task.CompletedTask;
        }
    }
}
