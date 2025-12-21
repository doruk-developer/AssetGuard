using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // Gerekli Servisler Tanımlanıyor
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;

        // TEK VE BİRLEŞTİRİLMİŞ CONSTRUCTOR (HEPSİNİ BURADA ALIYORUZ)
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        // ==========================================
        // 1. GİRİŞ İŞLEMLERİ (LOGIN)
        // ==========================================
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            // "admin", "123" gibi basit bilgileri kontrol et
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
            return View();
        }

        // ==========================================
        // 2. ÇIKIŞ İŞLEMİ (LOGOUT)
        // ==========================================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // ==========================================
        // 3. ŞİFREMİ UNUTTUM (FORGOT PASSWORD)
        // ==========================================
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Lütfen e-posta adresinizi giriniz.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return View("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Linki oluşturuyoruz
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token = token, email = user.Email }, protocol: Request.Scheme);

            // --- DÜZELTME BURADA ---
            // Linki düz yazı olarak değil, HTML <a> etiketi içine koyuyoruz.
            // Böylece link ne kadar uzun olursa olsun bozulmaz.

            string emailBody = $@"
        <h3>Şifre Yenileme Talebi</h3>
        <p>Merhaba,</p>
        <p>Hesabınız için şifre sıfırlama talebinde bulundunuz.</p>
        <p>Aşağıdaki linke tıklayarak yeni şifrenizi belirleyebilirsiniz:</p>
        <br>
        <a href='{callbackUrl}' style='background-color:#007bff; color:white; padding:10px 20px; text-decoration:none; border-radius:5px;'>Şifremi Sıfırla</a>
        <br><br>
        <small>Eğer bu talebi siz yapmadıysanız, bu maili dikkate almayınız.</small>
    ";
            // -----------------------

            await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", emailBody);

            return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // A) Sayfa Açılırken (Veriyi Yakalar ve Ekrana Taşır)
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            // Eğer link bozuksa veya parametreler eksikse hata sayfasına at
            if (token == null || email == null)
            {
                // Burası önemli: Parametre yoksa formu hiç gösterme!
                return Content("HATA: Şifre sıfırlama linki geçersiz veya hatalı. Lütfen maildeki linke tekrar tıklayın.");
            }

            // Modeli doldurup sayfaya gönderiyoruz
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            // Model geçerli mi? (Şifreler uyuşuyor mu, boş alan var mı?)
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }

            // Hata varsa
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // ==========================================
        // 5. ŞİFRE DEĞİŞTİRME (İÇERİDEN - LOGIN OLMUŞ KULLANICI İÇİN)
        // ==========================================

    }

}