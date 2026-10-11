using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::ScentSphere.Domain.Common;
using global::ScentSphere.Domain.Enums;

namespace ScentSphere.Domain.Entities
{
    
    public class VerificationToken : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public string TokenHash { get; set; } = string.Empty;

        public TokenType TokenType { get; set; }

        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; }
    }
}
