using System;
using System.ComponentModel.DataAnnotations;

namespace ECommerceView.Models.Account
{
    public class AccountViewModel
    {
        public long Id { get; set; }
        public string IdentityServerID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GsmNumber { get; set; }
        public string Email { get; set; }
        public string IdentityNumber { get; set; }
        public string RegistrationAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}