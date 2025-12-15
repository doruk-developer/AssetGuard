using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IEmployeeService _employeeService;
        private readonly IAssignmentService _assignmentService; // YENİ: Kayıt için lazım

        // Constructor'da 3 servisi de istiyoruz
        public AssignmentController(IAssetService assetService, IEmployeeService employeeService, IAssignmentService assignmentService)
        {
            _assetService = assetService;
            _employeeService = employeeService;
            _assignmentService = assignmentService;
        }

        public IActionResult Index()
        {
            // Veritabanındaki tüm zimmet geçmişini detaylıca çek
            // TGetAllWithDetails() metodunu EFAssetDal'da oluşturmuştuk.
            var values = _assignmentService.TGetAllWithDetails();

            return View(values);
        }

        // SAYFAYI AÇMA (GET)
        [HttpGet]
        public IActionResult Create()
        {
            var model = new AssignmentAddViewModel
            {
                // Dropdownları dolduruyoruz
                AssetList = _assetService.TGetAll(),
                EmployeeList = _employeeService.TGetAll(),
                Assignment = new Assignment() // Boş nesne
            };
            return View(model);
        }

        // KAYDETME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Create(AssignmentAddViewModel model)
        {
            // 1. Zimmet tarihini şu an olarak ayarla
            model.Assignment.AssignedDate = DateTime.Now;

            // 2. Servisi kullanarak veritabanına kaydet
            _assignmentService.TAdd(model.Assignment);

            // 3. İşlem bitince anasayfaya veya listeye dön
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Return()
        {
            return View();
        }
    }
}