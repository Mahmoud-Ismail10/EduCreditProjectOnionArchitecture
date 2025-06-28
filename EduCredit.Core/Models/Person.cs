using EduCredit.Core.Chat;
using EduCredit.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduCredit.Core.Models
{
    public class Person : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string NationalId { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; } // enum
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();//Navigation Prop
        public ICollection<ChatMessage> ChatMessages { get; set; } = new HashSet<ChatMessage>();
    }
}
