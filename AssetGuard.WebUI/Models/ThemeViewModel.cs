namespace AssetGuard.WebUI.Models
{
    // Bu sınıf sadece arayüz ile controller arasında veri taşır, 
    // veritabanıyla bir bağı yoktur, hata çıkarmaz.
    public class ThemeViewModel
    {
        public string SidebarColor { get; set; }
        public bool IsDarkMode { get; set; }
    }
}