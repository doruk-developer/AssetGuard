using AssetGuard.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            // Veritabanından tüm personeli Departman bilgisiyle birlikte çekiyoruz
            var values = _employeeService.TGetAll();
            return View(values);
        }

        // 2. Yeni Personel Ekleme
        public IActionResult Create()
        {
            return View();
        }
    }
}