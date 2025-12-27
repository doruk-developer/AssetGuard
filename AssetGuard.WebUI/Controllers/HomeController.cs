using AssetGuard.Business.Abstract;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // --- 1. ALANLAR (FIELDS) - Sadece birer kez tanýmlanmalý ---
        private readonly IAssetService _assetService;
        private readonly IReportService _reportService;
        private readonly IAssignmentService _assignmentService;
        private readonly IEmployeeService _employeeService;
        private readonly ICategoryService _categoryService; // Filtreler için eklendi

        // --- 2. CONSTRUCTOR (YAPICI METOT) - Hepsini tek seferde enjekte ediyoruz ---
        public HomeController(
            IAssetService assetService,
            IReportService reportService,
            IAssignmentService assignmentService,
            IEmployeeService employeeService,
            ICategoryService categoryService)
        {
            _assetService = assetService;
            _reportService = reportService;
            _assignmentService = assignmentService;
            _employeeService = employeeService;
            _categoryService = categoryService;
        }

        // --- 3. INDEX ACTION (SAYFA AÇILIÞI) ---
        public IActionResult Index()
        {
            // A. Mevcut Ýstatistikleri Çek
            var toplamUrun = _assetService.TGetTotalAssetCount();
            var toplamDeger = _assetService.TGetTotalInventoryValue();
            var personelSayisi = _employeeService.TGetAll().Count;
            // Not: Status iliþkisini manager'da çözdüðümüz için burayý sadeleþtirebilirsin veya böyle býrakabilirsin.
            // Eðer hata alýrsan Manager'a yeni metot yazmamýz gerekebilir, þimdilik eski kodunu korudum:
            var arizaliSayisi = _assetService.TGetAssetsByStatus("Arýzalý").Count;

            // B. Grafik Verilerini Çek
            var categoryReport = _reportService.GetAssetDistributionReport();
            var expenseReport = _reportService.GetMonthlyExpenseReport();

            // C. Tablo Verileri (Son 5 Kayýt)
            var sonZimmetler = _assignmentService.TGetAllWithDetails().Take(5).ToList();
            var yeniUrunler = _assetService.TGetAllWithDetails()
                                           .OrderByDescending(x => x.Id)
                                           .Take(5)
                                           .ToList();

            // D. Verileri Kutuya (ViewModel) Doldur
            var model = new DashboardViewModel
            {
                // KPI Kutularý
                TotalAssetCount = toplamUrun,
                TotalInventoryValue = toplamDeger,
                TotalEmployeeCount = personelSayisi,
                TotalFaultyCount = arizaliSayisi,

                // Grafikler
                CategoryLabels = categoryReport.Select(x => x.CategoryName).ToList(),
                CategoryValues = categoryReport.Select(x => x.AssetCount).ToList(),
                MonthlyLabels = expenseReport.Select(x => x.MonthName).ToList(),
                MonthlyValues = expenseReport.Select(x => x.TotalAmount).ToList(),

                // Tablolar
                LastAssignments = sonZimmetler,
                NewAssets = yeniUrunler,

                // --- FÝLTRE ÝÇÝN KATEGORÝ LÝSTESÝ (ViewBag Yerine Buradan) ---
                CategoryList = _categoryService.TGetAll()
            };

            // E. Kutuyu Sayfaya Gönder
            return View(model);
        }

        // Filtrelenmiþ Ýstatistikleri Getir (AJAX için)
        [HttpGet]
        public IActionResult GetFilterStats(int year, int? categoryId)
        {
            // Business katmanýndaki DTO dönen metodu çaðýrýyoruz
            var stats = _assetService.GetDashboardStatistics(year, categoryId);
            return Json(stats);
        }
    }
}

