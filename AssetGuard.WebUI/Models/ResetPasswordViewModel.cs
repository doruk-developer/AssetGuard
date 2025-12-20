using System.ComponentModel.DataAnnotations;

namespace AssetGuard.WebUI.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        [MinLength(3, ErrorMessage = "Şifre en az 3 karakter olmalı.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmPassword { get; set; }
    }
}
