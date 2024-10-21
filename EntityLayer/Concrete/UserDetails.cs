using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace EntityLayer.Concrete
{
    public class UserDetails
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserPhone { get; set; }
        public string UserMail { get; set; }
        public int CountryId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsActive { get; set; }
        public Country Country { get; set; }
    }
}