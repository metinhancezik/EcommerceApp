using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class UserAuth
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime LastLoginTime { get; set; } = DateTime.UtcNow;
        public int FailedLoginAttempts { get; set; }
        public bool IsLocked { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneConfirmed { get; set; }
        public DateTime LockoutEndTime { get; set; }
        public UserDetails User { get; set; }
    }
}
