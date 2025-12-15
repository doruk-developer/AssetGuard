using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IEmployeeDal
    {
        List<Employee> GetAll();
        Employee GetById(int id);



        // --- Personellere Eklenen CRUD İmzaları ---
        void Add(Employee entity);
        void Update(Employee entity);
        void Delete(Employee entity);
    }
}
