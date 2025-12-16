// Tanım: IEmployeeService implementasyonu ile iş kurallarını uygulayan ve DAL operasyonlarını yönlendiren ana iş sınıfıdır.

using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IEmployeeDal _employeeDal;

        public EmployeeManager(IEmployeeDal employeeDal)
        {
            _employeeDal = employeeDal;
        }

        public List<Employee> TGetAll()
        {
            return _employeeDal.GetAll();
        }

        // --- YENİ EKLENENLER ---

        public void TAdd(Employee entity)
        {
            _employeeDal.Add(entity);
        }

        public void TDelete(Employee entity)
        {
            _employeeDal.Delete(entity);
        }

        public void TUpdate(Employee entity)
        {
            _employeeDal.Update(entity);
        }

        public Employee TGetById(int id)
        {
            return _employeeDal.GetById(id);
        }
    }
}