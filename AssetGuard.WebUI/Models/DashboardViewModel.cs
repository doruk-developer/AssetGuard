//Bu model, web uygulamasının gösterge paneli (dashboard) görünümü için veri yapısını temsil eder.  
//(Amacımız veritabanından gelen verileri bu model aracılığıyla göstermek.)

using AssetGuard.Entity; // Asset ve Assignment sınıfları için gerekli

namespace AssetGuard.WebUI.Models
{
    // Bu model, Dashboard (Ana Sayfa) üzerindeki tüm kutu, grafik ve tabloların verisini taşır.
    public class DashboardViewModel
    {
        // --- 1. KPI KUTULARI (Sayısal Veriler) ---
        public int TotalAssetCount { get; set; }        // Toplam Demirbaş
        public decimal TotalInventoryValue { get; set; } // Toplam Para
        public int TotalEmployeeCount { get; set; }     // Toplam Personel
        public int TotalFaultyCount { get; set; }       // Arızalı Sayısı

        // --- 2. GRAFİK VERİLERİ (Listeler) ---

        // Daire Grafik (Kategoriler)
        public List<string> CategoryLabels { get; set; }
        public List<int> CategoryValues { get; set; }

        // Eğri Grafik (Aylık Harcama)
        public List<string> MonthlyLabels { get; set; }
        public List<decimal> MonthlyValues { get; set; }

        // --- 3. TABLO VERİLERİ (Varlık Listeleri) ---

        // Son Zimmet Hareketleri Tablosu için
        public List<Assignment> LastAssignments { get; set; }

        // Envantere Yeni Girenler Listesi için
        public List<Asset> NewAssets { get; set; }
    }
}
