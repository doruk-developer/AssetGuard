using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    public class AssetAddViewModel
    {
        // Formdan gelecek asıl veri
        public Asset Asset { get; set; }

        // Dropdownları dolduracak listeler
        public List<Category> CategoryList { get; set; }
        public List<AssetStatus> StatusList { get; set; }
    }
}
