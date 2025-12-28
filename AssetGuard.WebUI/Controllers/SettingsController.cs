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
            // Sayfa ilk açıldığında varsayılan sekmeyle (view) açılması için boş model gönderiyoruz.
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public IActionResult SaveThemeSettings([FromBody] UserThemeSettings settings)
        {
            try
            {
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName)) return Json(new { success = false, message = "Kullanıcı tanınamadı." });

                // 1. Klasör Yolu
                string folderPath = Path.Combine(_env.ContentRootPath, "ThemeData");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 2. Dosya Yolu
                string filePath = Path.Combine(folderPath, $"{userName}_settings.json");

                // 3. Yazma İşlemi
                string jsonContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(filePath, jsonContent);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Dosya yazma hatası: " + ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            // Hata olsa da olmasa da bu bir "security" sekmesi işlemidir.
            model.ActiveTab = "security";

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi!";
                // Başarı durumunda inputlar temizlensin ama yine bu sekmede kalsın
                return View("Index", new ChangePasswordViewModel { ActiveTab = "security" });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code == "PasswordMismatch")
                    {
                        // İstediğin özel hata mesajı
                        TempData["ErrorMessage"] = "Mevcut şifre hatalı girilmiştir. Lütfen doğru giriş sağlayınız.";
                    }
                    else
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View("Index", model);
        }
    }
}