using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Application.DTOs.Auth
{
    public class VerifyEmailRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Otp { get; set; } = string.Empty;
    }
}
