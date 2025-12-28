using System.ComponentModel.DataAnnotations;

namespace AssetGuard.WebUI.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Mevcut Şifre")]
        [Required(ErrorMessage = "Mevcut şifrenizi girmeniz gereklidir.")]
        public string CurrentPassword { get; set; }

        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        [MinLength(3, ErrorMessage = "Şifre en az 3 karakter olmalı.")]
        public string NewPassword { get; set; }

        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmPassword { get; set; }

        // Varsayılan olarak 'view' (Görünüm) sekmesi açılsın. Şifre değiştirme hata mesajı için.
        public string ActiveTab { get; set; } = "view";
    }
}
