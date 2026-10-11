using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScentSphere.Domain.Entities
{
    using global::ScentSphere.Domain.Common;
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool IsEmailVerified { get; set; }

        public bool IsActive { get; set; } = true;

        public Guid RoleId { get; set; }

        public Role Role { get; set; } = null!;

        public DateTime? LastLoginAt { get; set; }
    }
}
