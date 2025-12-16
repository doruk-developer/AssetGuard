using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IEmployeeService
    {
        List<Employee> TGetAll();

        // --- EKLENEN YENİ METOTLAR ---
        void TAdd(Employee entity);
        void TDelete(Employee entity);
        void TUpdate(Employee entity);
        Employee TGetById(int id);
    }
}
