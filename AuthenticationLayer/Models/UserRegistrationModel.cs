using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLayer.Models
{
    public class UserRegistrationModel
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserMail { get; set; }
        public string UserPhone { get; set; }
        public int CountryId { get; set; }
        public string Password { get; set; }
    }
}
