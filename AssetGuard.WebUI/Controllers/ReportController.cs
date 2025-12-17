using AssetGuard.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class ReportController : Controller
    {
        // --- SERVİSLERİ TANIMLIYORUZ ---
        private readonly IAssignmentService _assignmentService; // Zimmet raporu için
        private readonly IAssetService _assetService;           // Diğer raporlar için (YENİ EKLENDİ)

        // --- CONSTRUCTOR INJECTION ---
        public ReportController(IAssignmentService assignmentService, IAssetService assetService)
        {
            _assignmentService = assignmentService;
            _assetService = assetService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(string type)
        {
            string reportTitle = "Rapor Detayı";
            string reportDescription = "";

            // Veri taşıyıcı: Farklı tipte listeler gelebileceği için 'object' kullanıyoruz
            object reportData = null;

            switch (type)
            {
                case "zimmet":
                    reportTitle = "Zimmet Durumu Özet Raporu";
                    reportDescription = "Personel üzerindeki aktif zimmetlerin listesi.";

                    // Aktif zimmetleri çek
                    var allAssignments = _assignmentService.TGetAllWithDetails();
                    reportData = allAssignments.Where(x => x.ReturnDate == null).ToList();
                    break;

                case "garanti":
                    reportTitle = "Garantisi Yaklaşan Ürünler";
                    reportDescription = "Garanti süresi önümüzdeki 30 gün içinde bitecek (veya bitmiş) cihazlar.";

                    // YENİ: AssetService'den garanti verisini çek
                    reportData = _assetService.TGetAssetsExpiringSoon(30);
                    break;

                case "mali":
                    reportTitle = "Mali Envanter Değeri";
                    reportDescription = "Tüm demirbaşların maliyet analizi.";

                    // YENİ: Tüm listeyi çek (View tarafında toplatacağız)
                    reportData = _assetService.TGetAllWithDetails();
                    break;

                case "ariza":
                    reportTitle = "Arıza & Onarım Geçmişi";
                    reportDescription = "Şu an 'Arızalı' veya 'Serviste' durumunda olan cihazlar.";

                    // YENİ: Sadece arızalıları getir
                    reportData = _assetService.TGetAssetsByStatus("Arızalı");
                    break;

                default:
                    reportTitle = "Genel Rapor";
                    break;
            }

            // Başlıkları View'a taşı
            ViewData["ReportTitle"] = reportTitle;
            ViewData["ReportDesc"] = reportDescription;
            ViewData["ReportType"] = type;

            // Veriyi View'a gönder
            return View(reportData);
        }
    }
}