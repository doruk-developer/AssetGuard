using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class DepartmentController : Controller
    {
        // Departman Listesi
        public IActionResult Index()
        {
            return View();
        }
    }
}