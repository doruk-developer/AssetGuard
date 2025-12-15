// Demirbaşları yönetmek amacıyla oluşturulmuş bir Controller
// İsteği Alır: Kullanıcı tarayıcıda /Asset/Index dediğinde bu Controller'ın Index() metodu çalışır.
// Karar Verir: Veritabanına doğrudan gitmez (Bu amatörlüktür). Veriyi getirme görevini IAssetService'e devreder (N-Tier Kuralı).
// Veriyi Hazırlar: IAssetService'ten gelen listeyi alır. Eğer gerekiyorsa bu listeyi sıralar, filtreler veya bir ViewModel'e koyar.
// View'a Gönderir: Hazırlanan listeyi (return View(values)) alır ve HTML şablonuna (Views/Asset/Index.cshtml) yollar.


using AssetGuard.Business.Abstract;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly ICategoryService _categoryService;
        private readonly IAssetStatusService _assetStatusService;

        // 3 Servisi birden talep ediyoruz(Bu üç servisi, görüntülenecek tabloda birden çok bölüm olduğu için talep ediyoruz)
        public AssetController(IAssetService assetService, ICategoryService categoryService, IAssetStatusService assetStatusService)
        {
            _assetService = assetService;
            _categoryService = categoryService;
            _assetStatusService = assetStatusService;
        }

        // LİSTELEME
        public IActionResult Index()
        {
            var values = _assetService.TGetAll();
            return View(values);
        }

        // EKLEME SAYFASINI GETİR (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // 1. Boş bir ViewModel oluştur
            var model = new AssetAddViewModel();

            // 2. Dropdownların içini doldur (Veritabanından çekerek)
            model.CategoryList = _categoryService.TGetAll();
            model.StatusList = _assetStatusService.TGetAll();

            // 3. Modeli sayfaya gönder
            return View(model);
        }

        // EKLEME İŞLEMİNİ YAP (POST)
        [HttpPost]
        public IActionResult Create(AssetAddViewModel model)
        {
            // Formdan gelen veriyi (model.Asset) veritabanına kaydet
            _assetService.TAdd(model.Asset);

            // İş bitince listeye geri dön
            return RedirectToAction("Index");
        }


        // Formdan gelen id'ye sahip demirbaş kaydını, delete butonuna tıklanınca silmak için
        public IActionResult Delete(int id)
        {
            // 1. Önce silinecek veriyi bul
            var value = _assetService.TGetById(id);

            // 2. Veriyi sil
            _assetService.TDelete(value);

            // 3. Listeye geri dön
            return RedirectToAction("Index");
        }

        // Fprmdan mavi güncelle butonuna tıklanınca, ilgili demirbaş kaydını düzenleme sayfasına getirmek için
        // Elbette bu Controller metodlarının bir de View tarafı var (Views/Asset/Edit.cshtml). İşte güncelleme sayfası orada hazırlanıyor.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // 1. Düzenlenecek veriyi veritabanından bul
            var value = _assetService.TGetById(id);

            // 2. ViewModel'i hazırla ve veriyi içine koy
            var model = new AssetAddViewModel();
            model.Asset = value; // Mevcut veriyi modele yüklüyoruz

            // 3. Dropdownları doldur (Create'deki gibi)
            model.CategoryList = _categoryService.TGetAll();
            model.StatusList = _assetStatusService.TGetAll();

            return View(model);
        }

        // GÜNCELLEME İŞLEMİNİ KAYDET (POST)
        [HttpPost]
        public IActionResult Edit(AssetAddViewModel model)
        {
            // Formdan gelen güncellenmiş veriyi kaydet
            _assetService.TUpdate(model.Asset);

            return RedirectToAction("Index");
        }
    }
}
