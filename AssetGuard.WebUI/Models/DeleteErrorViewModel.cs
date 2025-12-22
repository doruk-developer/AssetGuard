namespace AssetGuard.WebUI.Models
{
    public class DeleteErrorViewModel
    {
        public string EntityName { get; set; } // Örn: "Bilgisayar Kategorisi"
        public string ErrorMessage { get; set; } // Örn: "Bu kategoriye bağlı ürünler var"
        public List<string> RelatedItems { get; set; } // Bağlı ürünlerin isimleri (MacBook, Dell XPS...)
        public string BackUrl { get; set; } // "Geri Dön" butonu nereye gidecek?
    }
}
