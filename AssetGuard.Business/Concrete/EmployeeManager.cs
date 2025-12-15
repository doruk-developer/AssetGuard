// Tanım: IEmployeeService implementasyonu ile iş kurallarını uygulayan ve DAL operasyonlarını yönlendiren ana iş sınıfıdır.

using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    // Tanım: IEmployeeService implementasyonu ile iş kurallarını uygulayan ve DAL operasyonlarını yönlendiren ana iş sınıfıdır.
    public class EmployeeManager : IEmployeeService
    {
        private readonly IEmployeeDal _employeeDal;

        public EmployeeManager(IEmployeeDal employeeDal)
        {
            _employeeDal = employeeDal;
        }

        // C - CREATE
        public void TAdd(Employee entity)
        {
            // İleride buraya iş kuralı (Örn: TC Kimlik No Tekrarı Kontrolü) eklenebilir.
            _employeeDal.Add(entity);
        }

        // R - READ (Tümünü Getir)
        public List<Employee> TGetAll()
        {
            return _employeeDal.GetAll();
        }

        // R - READ (Tek Kayıt Getir)
        public Employee TGetById(int id)
        {
            return _employeeDal.GetById(id);
        }

        // U - UPDATE
        public void TUpdate(Employee entity)
        {
            // İleride buraya kural (Örn: Eski departmanı kaydet) eklenebilir.
            _employeeDal.Update(entity);
        }

        // D - DELETE
        public void TDelete(Employee entity)
        {
            _employeeDal.Delete(entity);
        }
    }
}