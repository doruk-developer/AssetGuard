using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        // 1. Personel Listesi
        public IActionResult Index()
        {
            return View();
        }

        // 2. Yeni Personel Ekleme
        public IActionResult Create()
        {
            return View();
        }
    }
}