using AssetGuard.Business.Abstract;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // Servisleri Tanýmlýyoruz
        private readonly IAssetService _assetService;
        private readonly IReportService _reportService;
        private readonly IAssignmentService _assignmentService; // YENÝ
        private readonly IEmployeeService _employeeService;     // YENÝ

        // Constructor Injection: 4 servisi birden talep ediyoruz (DÜZELTÝLEN KISIM)
        public HomeController(IAssetService assetService, IReportService reportService, IAssignmentService assignmentService, IEmployeeService employeeService)
        {
            _assetService = assetService;
            _reportService = reportService;
            _assignmentService = assignmentService;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            // 1. Genel Ýstatistikleri Çek
            var toplamUrun = _assetService.TGetTotalAssetCount();
            var toplamDeger = _assetService.TGetTotalInventoryValue();
            var personelSayisi = _employeeService.TGetAll().Count;
            var arizaliSayisi = _assetService.TGetAssetsByStatus("Arýzalý").Count;

            // 2. Grafik Verilerini Çek
            var categoryReport = _reportService.GetAssetDistributionReport();
            var expenseReport = _reportService.GetMonthlyExpenseReport();

            // 3. Tablo Verileri (Son 5 Kayýt) -- (EKSÝK OLAN KISIM EKLENDÝ)
            var sonZimmetler = _assignmentService.TGetAllWithDetails().Take(5).ToList();
            var yeniUrunler = _assetService.TGetAllWithDetails()
                                           .OrderByDescending(x => x.Id)
                                           .Take(5)
                                           .ToList();

            // 4. Verileri Kutuya (ViewModel) Doldur
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
                NewAssets = yeniUrunler
            };

            // 5. Kutuyu Sayfaya Gönder
            return View(model);
        }
    }
}