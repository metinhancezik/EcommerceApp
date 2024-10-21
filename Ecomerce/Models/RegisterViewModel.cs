using System.ComponentModel.DataAnnotations;

namespace ECommerceView.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon Numarası")]
        public string GsmNumber { get; set; }

        [Required(ErrorMessage = "Kimlik numarası zorunludur.")]
        [Display(Name = "Kimlik Numarası")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "Adres alanı zorunludur.")]
        [Display(Name = "Adres")]
        public string RegistrationAddress { get; set; }

        [Required(ErrorMessage = "Şehir alanı zorunludur.")]
        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ülke alanı zorunludur.")]
        [Display(Name = "Ülke")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Posta kodu zorunludur.")]
        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}