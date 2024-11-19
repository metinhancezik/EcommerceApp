using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Metrics;
using System.Numerics;

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
        public int RoleId { get; set; }
        public long? VendorId { get; set; }

        [ForeignKey("VendorId")]
        public Vendors Vendor { get; set; }

        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
        public Country Country { get; set; }

    }
}