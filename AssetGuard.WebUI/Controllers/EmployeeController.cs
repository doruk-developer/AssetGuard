using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService; // Departman Dropdown'ı için lazım

        // Constructor Injection
        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        // LİSTELEME
        public IActionResult Index()
        {
            var values = _employeeService.TGetAll();
            return View(values);
        }

        // YENİ PERSONEL EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // 1. Departman listesini çek
            var departments = _departmentService.TGetAll();

            // 2. ViewModel'i oluştur ve doldur
            var model = new EmployeeAddViewModel
            {
                Employee = new Employee(),
                DepartmentList = departments
            };

            return View(model);
        }

        // YENİ PERSONELİ KAYDETME (POST)
        [HttpPost]
        public IActionResult Create(EmployeeAddViewModel model)
        {
            // İleride ModelState.IsValid kontrolü yapılacak

            // Veriyi servise gönder ve kaydet
            _employeeService.TAdd(model.Employee);

            // İşlem bitince listeye geri dön
            return RedirectToAction("Index");
        }

        // --- SİLME İŞLEMİ ---
        public IActionResult Delete(int id)
        {
            // 1. Silinecek personeli bul
            var value = _employeeService.TGetById(id);

            // 2. Sil
            _employeeService.TDelete(value);

            // 3. Listeye dön
            return RedirectToAction("Index");
        }

        // --- DÜZENLEME SAYFASI (GET) ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 1. Düzenlenecek personeli bul
            var value = _employeeService.TGetById(id);

            // 2. Departman listesini çek (Dropdown için)
            var departments = _departmentService.TGetAll();

            // 3. Modeli doldur (Mevcut verilerle)
            var model = new EmployeeAddViewModel
            {
                Employee = value,
                DepartmentList = departments
            };

            return View(model);
        }

        // --- DÜZENLEME KAYDI (POST) ---
        [HttpPost]
        public IActionResult Edit(EmployeeAddViewModel model)
        {
            // Güncellenmiş veriyi kaydet
            _employeeService.TUpdate(model.Employee);

            return RedirectToAction("Index");
        }
    }
}