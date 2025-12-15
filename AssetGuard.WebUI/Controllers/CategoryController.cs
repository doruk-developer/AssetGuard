using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        // Kategori Listesi
        public IActionResult Index()
        {
            return View();
        }
    }
}
