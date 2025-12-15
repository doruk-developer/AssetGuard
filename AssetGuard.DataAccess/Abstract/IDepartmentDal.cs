// Tanım: Department Entity'si için veritabanı erişim metod imzalarını barındırır (Repository Pattern).

using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IDepartmentDal
    {
        List<Department> GetAll();
    }
}
