using AssetGuard.Business.Abstract;
using ClosedXML.Excel;
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

        public IActionResult ExportToExcel(string type = "mali")
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Rapor");
                string fileName = "Rapor";
                int totalRows = 0;
                int colCount = 0;

                // 1. VERİLERİ DOLDURMA (Switch-Case)
                switch (type)
                {
                    case "zimmet":
                        fileName = "Aktif_Zimmet_Raporu";
                        colCount = 4;
                        var zimmetData = _assignmentService.TGetAllWithDetails().Where(x => x.ReturnDate == null).ToList();
                        totalRows = zimmetData.Count;
                        worksheet.Cell(1, 1).Value = "Personel";
                        worksheet.Cell(1, 2).Value = "Departman";
                        worksheet.Cell(1, 3).Value = "Ürün";
                        worksheet.Cell(1, 4).Value = "Veriliş Tarihi";
                        for (int i = 0; i < totalRows; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = zimmetData[i].Employee?.FirstName + " " + zimmetData[i].Employee?.LastName;
                            worksheet.Cell(i + 2, 2).Value = zimmetData[i].Employee?.Department?.Name ?? "-";
                            worksheet.Cell(i + 2, 3).Value = zimmetData[i].Asset?.AssetName;
                            worksheet.Cell(i + 2, 4).Value = zimmetData[i].AssignmentDate.ToString("dd.MM.yyyy");
                        }
                        break;

                    case "garanti":
                        fileName = "Garanti_Raporu";
                        colCount = 3;
                        var garantiData = _assetService.TGetAssetsExpiringSoon(30);
                        totalRows = garantiData.Count;
                        worksheet.Cell(1, 1).Value = "Ürün";
                        worksheet.Cell(1, 2).Value = "Seri No";
                        worksheet.Cell(1, 3).Value = "Garanti Bitiş";
                        for (int i = 0; i < totalRows; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = garantiData[i].AssetName;
                            worksheet.Cell(i + 2, 2).Value = garantiData[i].SerialNo;
                            worksheet.Cell(i + 2, 3).Value = garantiData[i].WarrantyEndDate?.ToString("dd.MM.yyyy");
                        }
                        break;

                    case "mali":
                        fileName = "Mali_Envanter_Degeri";
                        colCount = 3;
                        var maliData = _assetService.TGetAll();
                        totalRows = maliData.Count;
                        worksheet.Cell(1, 1).Value = "Ürün";
                        worksheet.Cell(1, 2).Value = "Kategori";
                        worksheet.Cell(1, 3).Value = "Fiyat (TL)";
                        for (int i = 0; i < totalRows; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = maliData[i].AssetName;
                            worksheet.Cell(i + 2, 2).Value = maliData[i].Category?.Name ?? "-";
                            worksheet.Cell(i + 2, 3).Value = maliData[i].Price;
                            worksheet.Cell(i + 2, 3).Style.NumberFormat.Format = "#,##0.00 \"TL\"";
                        }
                        break;

                    case "ariza":
                        fileName = "Ariza_Onarim_Raporu";
                        colCount = 3;
                        var arizaData = _assetService.TGetAssetsByStatus("Arızalı");
                        totalRows = arizaData.Count;
                        worksheet.Cell(1, 1).Value = "Ürün";
                        worksheet.Cell(1, 2).Value = "Durum";
                        worksheet.Cell(1, 3).Value = "Fiyat";
                        for (int i = 0; i < totalRows; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = arizaData[i].AssetName;
                            worksheet.Cell(i + 2, 2).Value = arizaData[i].Status?.Name;
                            worksheet.Cell(i + 2, 3).Value = arizaData[i].Price;
                            worksheet.Cell(i + 2, 3).Style.NumberFormat.Format = "#,##0.00 \"TL\"";
                        }
                        break;
                }

                // 2. PROFESYONEL TASARIM DOKUNUŞLARI
                if (totalRows > 0)
                {
                    var reportRange = worksheet.Range(1, 1, totalRows + 1, colCount);

                    // Başlık: Koyu Kurumsal Mavi Üstüne Beyaz Yazı
                    var header = worksheet.Range(1, 1, 1, colCount);
                    header.Style.Font.Bold = true;
                    header.Style.Fill.BackgroundColor = XLColor.FromHtml("#2F5597"); // Dark Blue
                    header.Style.Font.FontColor = XLColor.White;
                    header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // ZEBRA STRIPING: Açık Buz Mavisi (#E3EFFD)
                    for (int i = 2; i <= totalRows + 1; i++)
                    {
                        // İçerikleri dikeyde ortala (Şık durması için)
                        worksheet.Row(i).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        if (i % 2 == 0)
                        {
                            worksheet.Range(i, 1, i, colCount).Style.Fill.BackgroundColor = XLColor.FromHtml("#E3EFFD");
                        }
                    }

                    // KENARLIKLAR
                    // Tüm hücrelere ince iç çizgi
                    reportRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    reportRange.Style.Border.InsideBorderColor = XLColor.LightGray;

                    // EN DIŞA KALIN ÇERÇEVE (Sektör Standardı)
                    reportRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                    reportRange.Style.Border.OutsideBorderColor = XLColor.Black;

                    // Sütunları İçeriğe Göre Otomatik Genişlet
                    worksheet.Columns(1, colCount).AdjustToContents();
                }

                // 3. DOSYAYI OLUŞTUR VE GÖNDER
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    string fullFileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fullFileName);
                }
            }
        }
    }
}