namespace AssetGuard.Entity.Reports
{
    // Bu sınıf, Kategori bazlı dağılım raporunun sonucunu taşır.
    public class AssetDistributionReport
    {
        public string CategoryName { get; set; } // Kategori Adı
        public int AssetCount { get; set; }      // O kategoride kaç ürün var?
        public decimal TotalPrice { get; set; }  // O kategorideki ürünlerin toplam fiyatı
    }
}
