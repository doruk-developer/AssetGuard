using AssetGuard.Business.Abstract;
using AssetGuard.Entity;
using AssetGuard.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IAssetService _assetService;

        // Constructor Injection
        public CategoryController(ICategoryService categoryService, IAssetService assetService)
        {
            _categoryService = categoryService;
            _assetService = assetService;
        }

        // 1. LİSTELEME
        public IActionResult Index()
        {
            var model = new CategoryListViewModel
            {
                Categories = _categoryService.TGetAll(),
                // Başlangıçta hata yok, ErrorInfo null kalır
                ErrorInfo = null
            };
            return View(model);
        }

        // 2. YENİ EKLEME (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. YENİ EKLEME (POST)
        [HttpPost]
        public IActionResult Create(Category p)
        {
            _categoryService.TAdd(p);
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // 4. SİLME (Hata Yönetimi ve Pop-Up Mantığı Burada)
        public IActionResult Delete(int id)
        {
            try
            {
                var value = _categoryService.TGetById(id);
                if (value != null)
                {
                    _categoryService.TDelete(value);
                    TempData["Warning"] = "Kategori başarıyla silindi.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // HATA OLDU! ViewModel'i hata bilgileriyle dolduruyoruz.

                var category = _categoryService.TGetById(id);
                var relatedAssets = _assetService.TGetAll()
                                                 .Where(x => x.CategoryId == id)
                                                 .Select(x => x.AssetName + " (Seri No: " + x.SerialNo + ")") // Sadece isimleri alıyoruz
                                                 .ToList();

                var model = new CategoryListViewModel
                {
                    Categories = _categoryService.TGetAll(), // Listeyi tekrar çekiyoruz ki tablo boş kalmasın

                    // Hata Paketini Dolduruyoruz
                    ErrorInfo = new DeleteErrorViewModel
                    {
                        EntityName = category.Name,
                        ErrorMessage = "Bu kategori silinemez! Çünkü bu kategoriye bağlı demirbaşlar mevcuttur.",
                        RelatedItems = relatedAssets,
                        BackUrl = "/Category/Index"
                    }
                };

                return View("Index", model); // Index sayfasına hata paketiyle dönüyoruz
            }
        }

        // 5. GÜNCELLEME (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = _categoryService.TGetById(id);
            return View(value);
        }

        // 6. GÜNCELLEME (POST)
        [HttpPost]
        public IActionResult Edit(Category p)
        {
            _categoryService.TUpdate(p);
            TempData["Info"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}