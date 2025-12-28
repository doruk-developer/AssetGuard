//Bu model, web uygulamasının gösterge paneli (dashboard) görünümü için veri yapısını temsil eder.  
//(Amacımız veritabanından gelen verileri bu model aracılığıyla göstermek.)

using AssetGuard.Entity; // Asset ve Assignment sınıfları için gerekli

namespace AssetGuard.WebUI.Models
{
    public class DashboardViewModel
    {
        // 1. KPI KUTULARI (Değer tipleri olduğu için null olmazlar, sorun yok)
        public int TotalAssetCount { get; set; } // Toplam Demirbaş
        public decimal TotalInventoryValue { get; set; } // Toplam Para(Envanter Değeri)
        public int TotalEmployeeCount { get; set; } // Toplam Çalışan
        public int TotalFaultyCount { get; set; }  // Arızalı Sayısı

        // 2. Grafik Verileri (Listeler)
        // (UYARILARI GİDERMEK İÇİN "= new();" EKLİYORUZ)
        // Bu sayede listeler asla "null" olmaz, en kötü ihtimalle "boş liste" olur.
        // Kod patlamaz.

        // Daire Grafik (Kategoriler)
        public List<string> CategoryLabels { get; set; } = new();
        public List<int> CategoryValues { get; set; } = new();

        // Eğri Grafik (Aylık Harcama)
        public List<decimal> MonthlyValues { get; set; } = new();
        public List<string> MonthlyLabels { get; set; } = new();

        // Son Zimmet Hareketleri Tablosu için
        public List<Assignment> LastAssignments { get; set; } = new();

        // Envantere Yeni Girenler Listesi için
        public List<Asset> NewAssets { get; set; } = new();

        // Dashboard Filtre Kategorileri için
        public List<Category> CategoryList { get; set; } = new();

        // --- YENİ EKLENEN: GRAFİK TİPİ TERCİHİ ---
        // Ayarlar sayfasından gelen "bar" veya "doughnut" bilgisini taşır.
        public string ChartPreference { get; set; } = "doughnut";
    }
}




