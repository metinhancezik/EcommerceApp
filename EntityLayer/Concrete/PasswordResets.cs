using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class PasswordResets
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime RequestedAt { get; set; }
        public UserDetails User { get; set; }
    }
}
