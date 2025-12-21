using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        // 404 - Sayfa Bulunamadı Hatası
        public IActionResult Page404()
        {
            return View();
        }

        // 500 - Sunucu/Sistem Hatası (Bizim sabotajın düşeceği yer)
        public IActionResult Page500()
        {
            return View();
        }
    }
}
