// Demirbaşları yönetmek amacıyla oluşturulmuş bir Controller
// İsteği Alır: Kullanıcı tarayıcıda /Asset/Index dediğinde bu Controller'ın Index() metodu çalışır.
// Karar Verir: Veritabanına doğrudan gitmez (Bu amatörlüktür). Veriyi getirme görevini IAssetService'e devreder (N-Tier Kuralı).
// Veriyi Hazırlar: IAssetService'ten gelen listeyi alır. Eğer gerekiyorsa bu listeyi sıralar, filtreler veya bir ViewModel'e koyar.
// View'a Gönderir: Hazırlanan listeyi (return View(values)) alır ve HTML şablonuna (Views/Asset/Index.cshtml) yollar.


using AssetGuard.Business.Abstract;
using AssetGuard.WebUI.Models;
using ClosedXML.Excel; // Excel çıktısı alabilmek için
using Microsoft.AspNetCore.Mvc;

namespace AssetGuard.WebUI.Controllers
{
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly ICategoryService _categoryService;
        private readonly IAssetStatusService _assetStatusService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AssetController(IAssetService assetService,
                               ICategoryService categoryService,
                               IAssetStatusService assetStatusService,
                               IWebHostEnvironment hostEnvironment)
        {
            _assetService = assetService;
            _categoryService = categoryService;
            _assetStatusService = assetStatusService;
            _hostEnvironment = hostEnvironment;
        }

        // 1. LİSTELEME
        public IActionResult Index()
        {
            var values = _assetService.TGetAll();
            return View(values);
        }

        // 2. YENİ KAYIT (GET)
        [HttpGet]
        public IActionResult Create()
        {
            var model = new AssetAddViewModel
            {
                CategoryList = _categoryService.TGetAll(),
                StatusList = _assetStatusService.TGetAll()
            };
            return View(model);
        }

        // 3. YENİ KAYIT (POST)
        [HttpPost]
        public async Task<IActionResult> Create(AssetAddViewModel model)
        {
            if (model.Image != null)
            {
                string extension = Path.GetExtension(model.Image.FileName);
                string newImageName = Guid.NewGuid() + extension;
                string path = Path.Combine(_hostEnvironment.WebRootPath, "img/assets", newImageName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                model.Asset.ImageUrl = newImageName;
            }

            _assetService.TAdd(model.Asset);
            return RedirectToAction("Index");
        }

        // 4. DÜZENLEME (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var value = _assetService.TGetById(id);
            if (value == null) return NotFound();

            // Hayalet dosya kontrolü
            if (!string.IsNullOrWhiteSpace(value.ImageUrl))
            {
                var fullPath = Path.Combine(_hostEnvironment.WebRootPath, "img/assets", value.ImageUrl);
                if (!System.IO.File.Exists(fullPath))
                {
                    value.ImageUrl = null; // Fiziksel dosya yoksa veriyi boşalt
                }
            }

            var model = new AssetAddViewModel
            {
                Asset = value,
                CategoryList = _categoryService.TGetAll(),
                StatusList = _assetStatusService.TGetAll()
            };
            return View(model);
        }

        // 5. DÜZENLEME (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(AssetAddViewModel model)
        {
            // 1. ADIM: Veritabanındaki GERÇEK ve TAKİP EDİLEN nesneyi buluyoruz.
            // Bu nesne şu an EF'in elinde, yani "Tracked" durumda.
            var assetToUpdate = _assetService.TGetById(model.Asset.Id);

            if (assetToUpdate == null)
            {
                return NotFound();
            }

            // 2. ADIM: RESİM MANTIĞINI YÖNETELİM
            // Mevcut resim adını bir değişkene alalım
            string oldImageUrl = assetToUpdate.ImageUrl;

            if (model.IsImageRemoved) // Eğer "Kaldır" butonuna basıldıysa
            {
                if (!string.IsNullOrWhiteSpace(oldImageUrl))
                {
                    var path = Path.Combine(_hostEnvironment.WebRootPath, "img/assets", oldImageUrl);
                    if (System.IO.File.Exists(path)) { System.IO.File.Delete(path); }
                }
                assetToUpdate.ImageUrl = null; // Takip edilen nesneyi güncelliyoruz
            }
            else if (model.Image != null) // Eğer yeni bir resim yüklendiyse
            {
                // Eski resmi diskten silelim
                if (!string.IsNullOrWhiteSpace(oldImageUrl))
                {
                    var oldPath = Path.Combine(_hostEnvironment.WebRootPath, "img/assets", oldImageUrl);
                    if (System.IO.File.Exists(oldPath)) { System.IO.File.Delete(oldPath); }
                }

                // Yeni resmi kaydedelim
                string extension = Path.GetExtension(model.Image.FileName);
                string newImageName = Guid.NewGuid() + extension;
                string newPath = Path.Combine(_hostEnvironment.WebRootPath, "img/assets", newImageName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }
                assetToUpdate.ImageUrl = newImageName; // Takip edilen nesneyi güncelliyoruz
            }

            // 3. ADIM: DİĞER ALANLARI GÜNCELLEYELİM (MAPPING)
            // Formdan gelen (model.Asset) verileri, takip edilen (assetToUpdate) nesnesine tek tek aktarıyoruz.
            assetToUpdate.AssetName = model.Asset.AssetName;
            assetToUpdate.SerialNo = model.Asset.SerialNo;
            assetToUpdate.CategoryId = model.Asset.CategoryId;
            assetToUpdate.StatusId = model.Asset.StatusId;
            assetToUpdate.Price = model.Asset.Price;
            assetToUpdate.PurchaseDate = model.Asset.PurchaseDate;
            assetToUpdate.WarrantyEndDate = model.Asset.WarrantyEndDate;

            // 4. ADIM: GÜNCELLEMEYİ TETİKLE
            // Artık model.Asset'i (yabancıyı) değil, assetToUpdate'i (tanıdığı) gönderiyoruz.
            _assetService.TUpdate(assetToUpdate);

            return RedirectToAction("Index");
        }

        // 6. SİLME
        public IActionResult Delete(int id)
        {
            var value = _assetService.TGetById(id);
            if (value != null)
            {
                // Ürün silinirken dosyasını da diskten temizle
                if (!string.IsNullOrWhiteSpace(value.ImageUrl))
                {
                    var path = Path.Combine(_hostEnvironment.WebRootPath, "img/assets", value.ImageUrl);
                    if (System.IO.File.Exists(path)) { System.IO.File.Delete(path); }
                }
                _assetService.TDelete(value);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ExportExcel()
        {
            // 1. Verileri veritabanından çekiyoruz
            var assetList = _assetService.TGetAll();

            // 2. Bellekte bir Excel dosyası oluşturuyoruz
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Envanter Listesi");

                // Başlıkları yazıyoruz (Hücre koordinatları: Satır, Sütun)
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Demirbaş Adı";
                worksheet.Cell(1, 3).Value = "Seri No";
                worksheet.Cell(1, 4).Value = "Kategori";
                worksheet.Cell(1, 5).Value = "Durum";
                worksheet.Cell(1, 6).Value = "Fiyat (TL)";
                worksheet.Cell(1, 7).Value = "Alım Tarihi";

                // Başlık stilini profesyonelleştirelim (Kalın ve Gri Arka Plan)
                var headerRow = worksheet.Range("A1:G1");
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Verileri satır satır döngüyle yazıyoruz
                int rowCount = 2;
                foreach (var item in assetList)
                {
                    worksheet.Cell(rowCount, 1).Value = item.Id;
                    worksheet.Cell(rowCount, 2).Value = item.AssetName;
                    worksheet.Cell(rowCount, 3).Value = item.SerialNo;
                    worksheet.Cell(rowCount, 4).Value = item.Category?.Name ?? "-";
                    worksheet.Cell(rowCount, 5).Value = item.Status?.Name ?? "-";
                    worksheet.Cell(rowCount, 6).Value = item.Price;
                    worksheet.Cell(rowCount, 7).Value = item.PurchaseDate.ToString("dd.MM.yyyy");
                    rowCount++;
                }

                // Sütun genişliklerini içeriğe göre otomatik ayarla (Okunabilirlik için)
                worksheet.Columns().AdjustToContents();

                // 3. Dosyayı kullanıcıya gönderiyoruz
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    // Dosya ismine tarih ve saat ekleyerek benzersiz yapalım
                    string fileName = $"AssetGuard_Envanter_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}