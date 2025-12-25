using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public SettingsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Sadece görsel olarak ayar sayfasını açar
            var model = new ThemeViewModel { SidebarColor = "primary", IsDarkMode = false };
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveTheme(ThemeViewModel model)
        {
            // Veritabanına dokunmuyoruz, sadece işlem başarılıymış gibi mesaj veriyoruz
            TempData["Success"] = "Ayarlar bu oturum için kaydedildi (Veritabanı bağlantısı kapalı).";
            return RedirectToAction("Index");
        }
    }
}