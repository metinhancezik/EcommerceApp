using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdentityServerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string GsmNumber { get; set; }

        [Required]
        [StringLength(11)]
        public string IdentityNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string RegistrationAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}