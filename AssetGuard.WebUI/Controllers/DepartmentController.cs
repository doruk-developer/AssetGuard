using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var values = _departmentService.TGetAll();
            return View(values);
        }

        // --- YENİ DEPARTMAN EKLEME ---
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department p)
        {
            _departmentService.TAdd(p);
            return RedirectToAction("Index");
        }

        // --- SİLME ---
        public IActionResult Delete(int id)
        {
            var value = _departmentService.TGetById(id);
            _departmentService.TDelete(value);
            return RedirectToAction("Index");
        }

        // --- GÜNCELLEME ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = _departmentService.TGetById(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult Edit(Department p)
        {
            _departmentService.TUpdate(p);
            return RedirectToAction("Index");
        }
    }
}