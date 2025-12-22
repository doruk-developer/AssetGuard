using AssetGuard.Entity;

namespace AssetGuard.WebUI.Models
{
    public class EmployeeListViewModel
    {
        public List<Employee> Employees { get; set; }
        public DeleteErrorViewModel ErrorInfo { get; set; }
    }
}
