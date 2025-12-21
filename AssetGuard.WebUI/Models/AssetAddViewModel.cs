using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    public class AssetAddViewModel
    {
        public Asset Asset { get; set; }
        public List<Category> CategoryList { get; set; }
        public List<AssetStatus> StatusList { get; set; }
        public IFormFile Image { get; set; }

        // Kullanıcı Dropify'da 'Remove' tuşuna basarsa burası 'true' dönecek (Resim kaldırma opsiyonu için)
        public bool IsImageRemoved { get; set; }
    }
}
