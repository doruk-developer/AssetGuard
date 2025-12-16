// Tanım: Department Entity'si için veritabanı erişim metod imzalarını barındırır (Repository Pattern).

using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IDepartmentDal
    {
        List<Department> GetAll();

        // --- EKSİK OLAN BU METOTLARDI ---
        void Add(Department entity);
        void Delete(Department entity);
        void Update(Department entity);
        Department GetById(int id);
    }
}
