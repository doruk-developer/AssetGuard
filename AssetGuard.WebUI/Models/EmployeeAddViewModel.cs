//Hem Dropdown listesini dolduracak, hem de formdan gelecek veriyi taşıyacak bir nodel

using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    /// <summary>
    /// Personel Ekleme Formu için veri taşıyıcı (ViewModel).
    /// Form verisini (Employee) ve Dropdown listesini (DepartmentList) View'a taşır.
    /// </summary>
    public class EmployeeAddViewModel
    {
        // Formdan gelecek asıl veri
        public Employee Employee { get; set; }

        // Dropdownları dolduracak liste
        public List<Department> DepartmentList { get; set; }
    }
}
