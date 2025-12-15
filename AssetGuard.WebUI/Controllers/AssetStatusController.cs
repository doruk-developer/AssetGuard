using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class AssetStatusController : Controller
    {
        // Durum Listesi (Hurda, Sağlam vb.)
        public IActionResult Index()
        {
            return View();
        }
    }
}