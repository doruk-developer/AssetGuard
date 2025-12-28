using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IAssignmentService _assignmentService;
        private readonly IValidator<Employee> _employeeValidator;

        // Constructor Injection - Gerekli tüm servisler burada toplanmıştır.
        public EmployeeController(IEmployeeService employeeService,
                                  IDepartmentService departmentService,
                                  IAssignmentService assignmentService,
                                  IValidator<Employee> employeeValidator)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _assignmentService = assignmentService;
            _employeeValidator = employeeValidator;
        }

        // LİSTELEME
        public IActionResult Index()
        {
            var model = new EmployeeListViewModel
            {
                Employees = _employeeService.TGetAll(),
                ErrorInfo = null
            };
            return View(model);
        }

        // YENİ PERSONEL EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentService.TGetAll();
            var model = new EmployeeAddViewModel
            {
                Employee = new Employee(),
                DepartmentList = departments
            };
            return View(model);
        }

        // YENİ PERSONELİ KAYDETME (POST)
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeAddViewModel model)
        {
            // FluentValidation kontrolü (Asenkron)
            ValidationResult result = await _employeeValidator.ValidateAsync(model.Employee);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Employee." + error.PropertyName, error.ErrorMessage);
                }

                // --- Validasyon kontrolünden geçmemiş işlemler için uyarı mesajı ---
                TempData["Error"] = "Lütfen kırmızı ile belirtilen hatalı alanları düzeltiniz.";
                // -------------------------------

                model.DepartmentList = _departmentService.TGetAll();
                return View(model);
            }

            _employeeService.TAdd(model.Employee);
            TempData["Success"] = "Personel başarıyla sisteme eklendi.";
            return RedirectToAction("Index");
        }

        // SİLME İŞLEMİ
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
                var emp = _employeeService.TGetById(id);

                // Engelleyen zimmet kayıtlarını detaylarıyla birlikte çekiyoruz
                var relatedAssignments = _assignmentService.TGetAllWithDetails()
                                         .Where(x => x.EmployeeId == id)
                                         .Select(x => x.Asset?.AssetName + " (Tarih: " + x.AssignmentDate.ToString("dd.MM.yyyy") + ")")
                                         .ToList();

                var model = new EmployeeListViewModel
                {
                    Employees = _employeeService.TGetAll(),
                    ErrorInfo = new DeleteErrorViewModel
                    {
                        EntityName = emp.FirstName + " " + emp.LastName,
                        ErrorMessage = "Bu personel silinemez! Üzerinde zimmet kayıtları bulunmaktadır.",
                        RelatedItems = relatedAssignments,
                        BackUrl = "/Employee/Index"
                    }
                };

                return View("Index", model);
            }
        }

        // DÜZENLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = _employeeService.TGetById(id);
            if (value == null) return NotFound();

            var departments = _departmentService.TGetAll();
            var model = new EmployeeAddViewModel
            {
                Employee = value,
                DepartmentList = departments
            };
            return View(model);
        }

        // DÜZENLEME KAYDI (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeAddViewModel model)
        {
            // FluentValidation kontrolü (Asenkron)
            ValidationResult result = await _employeeValidator.ValidateAsync(model.Employee);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Employee." + error.PropertyName, error.ErrorMessage);
                }
                model.DepartmentList = _departmentService.TGetAll();
                return View(model);
            }

            _employeeService.TUpdate(model.Employee);
            TempData["Info"] = "Personel bilgileri güncellendi.";
            return RedirectToAction("Index");
        }
    }
}