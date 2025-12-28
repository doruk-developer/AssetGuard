using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;
using AssetGuard.Entity.DTOs;
using QRCoder;
using System.Globalization;

namespace AssetGuard.Business.Concrete
{
    public class AssetManager : IAssetService
    {
        // Business katmanı DataAccess katmanını kullanır (Bağımlılık)
        private readonly IAssetDal _assetDal;

        public AssetManager(IAssetDal assetDal)
        {
            _assetDal = assetDal;
        }

        public void TAdd(Asset entity)
        {
            // İleride buraya iş kuralları yazacağız (Örn: Fiyat negatif olamaz)
            _assetDal.Add(entity);
        }

        public void TDelete(Asset entity)
        {
            _assetDal.Delete(entity);
        }

        public List<Asset> TGetAll()
        {
            return _assetDal.GetAll();
        }

        public Asset TGetById(int id)
        {
            return _assetDal.GetById(id);
        }

        public void TUpdate(Asset entity)
        {
            _assetDal.Update(entity);
        }

        // Dashboard Verilerini Taşıyoruz
        public int TGetTotalAssetCount()
        {
            return _assetDal.GetTotalAssetCount();
        }

        public decimal TGetTotalInventoryValue()
        {
            return _assetDal.GetTotalInventoryValue();
        }

        // 1. Garanti Raporu Sorgusu için
        public List<Asset> TGetAssetsExpiringSoon(int days)
        {
            var expiryDate = DateTime.Now.AddDays(days);

            // Not: _assetDal.GetAll() metodunun "Include" (ilişkili verileri getirme) 
            // yaptığından emin olmalıyız (Önceki adımlarda yapmıştık).
            return _assetDal.GetAll()
                .Where(x => x.WarrantyEndDate != null && x.WarrantyEndDate <= expiryDate && x.WarrantyEndDate >= DateTime.Now)
                .ToList();
        }

        // 2. Arıza Raporu Sorgusu için
        public List<Asset> TGetAssetsByStatus(string statusName)
        {
            return _assetDal.GetAll()
                .Where(x => x.Status != null && x.Status.Name.Contains(statusName))
                .ToList();
        }

        // 3. Mali Durum Raporu Sorgusu için
        public List<Asset> TGetAllWithDetails()
        {
            // Zaten DAL katmanındaki GetAll metodu Include ile dolu getiriyordu.
            return _assetDal.GetAll();
        }

        public Asset? GetBySerialNo(string serialNo)
        {
            return _assetDal.GetAll().FirstOrDefault(x => x.SerialNo == serialNo && !x.IsDeleted);
        }

        public string GenerateQrCode(string detailUrl)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(detailUrl, QRCodeGenerator.ECCLevel.Q))
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);
                return $"data:image/png;base64,{Convert.ToBase64String(qrCodeAsPngByteArr)}";
            }
        }

        // Dashboard İstatistikleri(Filtreleme Sistemi) için
        public DashboardStatsDTO GetDashboardStatistics(int year, int? categoryId)
        {
            // 1. Veriyi Çek (Filtreleme Mantığı)
            var query = _assetDal.GetAll(); // Tüm veriyi getir

            // Yıl Filtresi
            if (year > 0)
            {
                query = query.Where(x => x.PurchaseDate.Year == year).ToList();
            }

            // Kategori Filtresi
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value).ToList();
            }

            // 2. İstatistikleri Hesapla (Business Logic)
            var stats = new DashboardStatsDTO
            {
                TotalAssetCount = query.Count,
                // Durum tablosunda "Arızalı" geçenleri say (Null kontrolü ile)
                BrokenAssetCount = query.Count(x => x.Status != null && x.Status.Name.Contains("Arızalı")),
                // Toplam Fiyatı TL formatına çevir
                TotalValue = query.Sum(x => x.Price).ToString("C0", new CultureInfo("tr-TR")),

                ChartLabels = new List<string> { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" },
                ChartValues = new List<decimal>()
            };

            // 3. Grafik Verisini Hazırla (Aylık Döngü)
            for (int i = 1; i <= 12; i++)
            {
                // O ayın toplam harcaması
                var monthTotal = query.Where(x => x.PurchaseDate.Month == i).Sum(x => x.Price);
                stats.ChartValues.Add(monthTotal);
            }

            return stats;
        }
    }
}