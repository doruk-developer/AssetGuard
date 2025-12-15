//Bu model, web uygulamasının gösterge paneli (dashboard) görünümü için veri yapısını temsil eder.  
//(Amacımız veritabanından gelen verileri bu model aracılığıyla göstermek.)

namespace AssetGuard.WebUI.Models
{
    public class DashboardViewModel
    {
        // Gerçekten veritabanından çekeceklerimiz
        public int TotalAssetCount { get; set; }
        public decimal TotalInventoryValue { get; set; }

        // Henüz Business katmanını yazmadığımız için şimdilik elle gireceklerimiz
        public int TotalEmployeeCount { get; set; }
        public int TotalFaultyCount { get; set; }
    }
}
