using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            // View oluşturmasan bile hata vermesin diye geçici metin:
            return View();
        }
    }
}