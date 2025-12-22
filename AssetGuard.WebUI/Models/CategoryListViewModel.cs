using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    public class CategoryListViewModel
    {
        // Tablo verisi
        public List<Category> Categories { get; set; }

        // Hata detay paketi (Eksik olan kısım buydu)
        public DeleteErrorViewModel ErrorInfo { get; set; }
    }
}