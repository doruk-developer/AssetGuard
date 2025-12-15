using AssetGuard.Business.Abstract; // Tanýmlama sayesinde, Controller, somut sýnýfa deðil, sadece sözleþmeye (arayüze) baðýmlý olabilir. (DI Prensibi)
using AssetGuard.WebUI.Models;      // Veritabanýndan gelen verileri taþýyýp görüntülememizi saðlayacak kutu kutu
using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC Çekirdek Kütüphanesini çaðýrýr. Bu, yazdýðýmýz kodun bir Web projesi (ASP.NET Core MVC) içinde çalýþmasýný saðlayan temel kütüphanedir.
//Bu üç satýr, HomeController'a "Ben bir web arayüzüyüm (AspNetCore.Mvc), sadece View'a özel veri yollarým (WebUI.Models), ama iþ kurallarý için Business katmanýna güvenirim (Business.Abstract)" dedirtir.


namespace AssetGuard.WebUI.Controllers
{
    public class HomeController : Controller
    {
        // 1. Servisi Tanýmla (Dependency Injection)
        private readonly IAssetService _assetService;

        // 2. Constructor'da Servisi Ýste (Program.cs bunu saðlayacak)
        public HomeController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        public IActionResult Index()
        {
            // 3. Veritabanýndan Gerçek Verileri Çek
            var toplamUrun = _assetService.TGetTotalAssetCount();
            var toplamDeger = _assetService.TGetTotalInventoryValue();

            // 4. Verileri Kutuya (ViewModel) Koy
            var model = new DashboardViewModel
            {
                TotalAssetCount = toplamUrun,
                TotalInventoryValue = toplamDeger,

                // Bunlarýn servisini henüz yazmadýk, þimdilik sahte kalsýn
                TotalEmployeeCount = 6,
                TotalFaultyCount = 1
            };

            // 5. Kutuyu Sayfaya Gönder
            return View(model);
        }
    }
}