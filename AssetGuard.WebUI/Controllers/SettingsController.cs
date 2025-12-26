using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AssetGuard.WebUI.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;

        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveThemeSettings([FromBody] UserThemeSettings settings)
        {
            try
            {
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName)) return Json(new { success = false, message = "Kullanıcı tanınamadı." });

                // 1. Klasör Yolu (Güvenli Erişim)
                string folderPath = Path.Combine(_env.ContentRootPath, "ThemeData");

                // Klasör yoksa oluştur (Hata önleyici)
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 2. Dosya Yolu: kullanıcıadı_settings.json
                string filePath = Path.Combine(folderPath, $"{userName}_settings.json");

                // 3. Yazma İşlemi
                string jsonContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(filePath, jsonContent);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata loglanabilir ama kullanıcıya basit mesaj dönüyoruz
                return Json(new { success = false, message = "Dosya yazma hatası: " + ex.Message });
            }
        }

        // ... ChangePassword metodun buraya gelecek (değişiklik yok) ...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) { return View("Index", model); }
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Şifre güncellendi.";
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }
    }
}