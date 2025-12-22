using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    public class DepartmentListViewModel
    {
        public List<Department> Departments { get; set; }
        public DeleteErrorViewModel ErrorInfo { get; set; }
    }
}