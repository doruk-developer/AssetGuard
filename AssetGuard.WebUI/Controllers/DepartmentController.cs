using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService; // Personel verilerini çekmek için

        public DepartmentController(IDepartmentService departmentService, IEmployeeService employeeService)
        {
            _departmentService = departmentService;
            _employeeService = employeeService;
        }

        // 1. LİSTELEME
        public IActionResult Index()
        {
            var model = new DepartmentListViewModel
            {
                Departments = _departmentService.TGetAll(),
                ErrorInfo = null
            };
            return View(model);
        }

        // 2. YENİ EKLEME (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. YENİ EKLEME (POST)
        [HttpPost]
        public IActionResult Create(Department p)
        {
            _departmentService.TAdd(p);
            TempData["Success"] = "Departman başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // 4. SİLME (Pop-Up Mantığı Burada)
        public IActionResult Delete(int id)
        {
            try
            {
                var value = _departmentService.TGetById(id);
                if (value != null)
                {
                    _departmentService.TDelete(value);
                    TempData["Warning"] = "Departman kaydı silindi.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // HATA: Bağlı personel var!
                var dept = _departmentService.TGetById(id);

                // Bu departmana bağlı çalışanların isimlerini çekiyoruz
                var relatedEmployees = _employeeService.TGetAll()
                                        .Where(x => x.DepartmentId == id)
                                        .Select(x => x.FirstName + " " + x.LastName)
                                        .ToList();

                var model = new DepartmentListViewModel
                {
                    Departments = _departmentService.TGetAll(), // Listeyi tazele
                    ErrorInfo = new DeleteErrorViewModel
                    {
                        EntityName = dept.Name,
                        ErrorMessage = "Bu departman silinemez! İçerisinde kayıtlı personel(ler) bulunmaktadır.",
                        RelatedItems = relatedEmployees,
                        BackUrl = "/Department/Index"
                    }
                };

                return View("Index", model); // Modal açılması için Index'e modelle dön
            }
        }

        // 5. GÜNCELLEME (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = _departmentService.TGetById(id);
            return View(value);
        }

        // 6. GÜNCELLEME (POST)
        [HttpPost]
        public IActionResult Edit(Department p)
        {
            _departmentService.TUpdate(p);
            TempData["Info"] = "Departman güncellendi.";
            return RedirectToAction("Index");
        }
    }
}