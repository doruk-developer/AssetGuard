// Assignment (Zimmet) Ekleme Formu için veri taşıyıcı (ViewModel).
// Controller'dan View'a (Sayfaya) hem formun kendisini hem de Dropdown listelerini taşır.

using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    public class AssignmentAddViewModel
    {
        // Formdan gelecek asıl veri (Zimmet Kaydı)
        public Assignment Assignment { get; set; }

        // Dropdownları dolduracak listeler
        public List<Asset> AssetList { get; set; }     // Hangi demirbaşı atıyoruz
        public List<Employee> EmployeeList { get; set; } // Hangi personele atıyoruz
    }
}
