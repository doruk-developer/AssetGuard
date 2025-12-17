using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // --- YENİ EKLENEN DETAY METODU ---
        [HttpGet]
        public IActionResult Details(string type)
        {
            // URL'den gelen ?type=zimmet gibi parametreyi yakalıyoruz.

            string reportTitle = "Rapor Detayı";
            string reportDescription = "";

            switch (type)
            {
                case "zimmet":
                    reportTitle = "Zimmet Durumu Özet Raporu";
                    reportDescription = "Personel üzerindeki aktif zimmetlerin listesi.";
                    break;
                case "garanti":
                    reportTitle = "Garantisi Yaklaşan Ürünler";
                    reportDescription = "Garanti süresi 30 gün içinde dolacak cihazlar.";
                    break;
                case "mali":
                    reportTitle = "Mali Envanter Değeri";
                    reportDescription = "Departman bazlı toplam demirbaş maliyet analizi.";
                    break;
                case "ariza":
                    reportTitle = "Arıza & Onarım Geçmişi";
                    reportDescription = "Servise gönderilen cihazların durum raporu.";
                    break;
                default:
                    reportTitle = "Genel Rapor";
                    break;
            }

            // Başlık ve Açıklamayı View'a taşıyoruz (ViewModel kullanmadan pratik yol)
            ViewData["ReportTitle"] = reportTitle;
            ViewData["ReportDesc"] = reportDescription;

            return View();
        }
    }
}