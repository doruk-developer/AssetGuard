using AssetGuard.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        // Constructor Injection
        public AccountController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // 1. Giriş Sayfasını Göster (GET)
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            // Giriş yaptıktan sonra nereye gideceğini aklında tutsun
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // 2. Giriş İşlemini Yap (POST)
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            // "admin", "123" bilgilerini kontrol et
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                // Başarılıysa, gitmek istediği yere (veya Ana Sayfaya) gönder
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }

            // Başarısızsa hatayı göster
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View();
        }

        // 3. Çıkış Yap
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}