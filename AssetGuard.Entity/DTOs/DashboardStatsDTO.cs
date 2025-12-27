// AJAX isteğine cevap dönerken tüm veritabanını değil, sadece grafiğin ve kartların ihtiyacı olan özet veriyi göndereceğiz.
// Bu DTO, dashboard için gerekli olan özet istatistikleri ve grafik verilerini içerecek(taşıyacak).

namespace AssetGuard.Entity.DTOs
{
    public class DashboardStatsDTO
    {
        // Kartlar İçin
        public int TotalAssetCount { get; set; }
        public int BrokenAssetCount { get; set; }
        public string TotalValue { get; set; }

        // Grafik İçin
        public List<string> ChartLabels { get; set; }
        public List<decimal> ChartValues { get; set; }
    }
}