using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IEmployeeDal _employeeDal;
        public EmployeeManager(IEmployeeDal employeeDal) { _employeeDal = employeeDal; }

        public List<Employee> TGetAll() { return _employeeDal.GetAll(); }
    }
}
