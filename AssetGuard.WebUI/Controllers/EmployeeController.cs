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
        private readonly IAssignmentService _assignmentService; // YENİ: Engelleyen zimmetleri görmek için

        // Constructor Injection
        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IAssignmentService assignmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _assignmentService = assignmentService;
        }

        // LİSTELEME
        public IActionResult Index()
        {
            var model = new EmployeeListViewModel
            {
                Employees = _employeeService.TGetAll(),
                ErrorInfo = null // Başlangıçta hata yok
            };
            return View(model);
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

            // Toastr Bildirimi
            TempData["Success"] = "Personel başarıyla sisteme eklendi.";

            // İşlem bitince listeye geri dön
            return RedirectToAction("Index");
        }

        // --- SİLME İŞLEMİ ---
        // Akıllı Hata Yönetimi: Eğer personelin üzerinde zimmet varsa pop-up açar.
        public IActionResult Delete(int id)
        {
            try
            {
                var value = _employeeService.TGetById(id);
                if (value != null)
                {
                    _employeeService.TDelete(value);
                    TempData["Warning"] = "Personel kaydı başarıyla silindi.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // HATA YÖNETİMİ: Personel silinemezse engelleyen zimmetleri buluyoruz.
                var emp = _employeeService.TGetById(id);

                // Bu personele ait aktif veya geçmiş zimmetleri bulalım
                var relatedAssignments = _assignmentService.TGetAllWithDetails()
                                         .Where(x => x.EmployeeId == id)
                                         .Select(x => x.Asset?.AssetName + " (Tarih: " + x.AssignedDate.ToString("dd.MM.yyyy") + ")")
                                         .ToList();

                var model = new EmployeeListViewModel
                {
                    Employees = _employeeService.TGetAll(), // Tablo verisini tazele
                    ErrorInfo = new DeleteErrorViewModel
                    {
                        EntityName = emp.FirstName + " " + emp.LastName,
                        ErrorMessage = "Bu personel silinemez! Çünkü üzerinde aktif veya geçmiş zimmet kayıtları bulunmaktadır.",
                        RelatedItems = relatedAssignments,
                        BackUrl = "/Employee/Index"
                    }
                };

                return View("Index", model); // Index sayfasına hata modeliyle dön
            }
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

            // Toastr Bildirimi
            TempData["Info"] = "Personel bilgileri güncellendi.";

            return RedirectToAction("Index");
        }
    }
}