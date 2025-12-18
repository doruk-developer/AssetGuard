using AssetGuard.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    [AllowAnonymous] // Kilitli olmayan tek kapı burası
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET: /Login/Index
        [HttpGet]
        public IActionResult Index()
        {
            // Eğer kullanıcı zaten giriş yapmışsa direkt Dashboard'a at
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Login/Index
        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            // Identity'nin 'Token' benzeri Cookie tabanlı doğrulaması
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Hatalı kullanıcı adı veya şifre.");
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
