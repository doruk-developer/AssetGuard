// Tanım: Department verilerinin iş katmanı seviyesinde sunulması için
// gerekli servis metod imzalarını barındırır.

using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IDepartmentService
    {
        List<Department> TGetAll();

        // --- Departman CRUD İşlemleri İçin ---
        void TAdd(Department entity);
        void TDelete(Department entity);
        void TUpdate(Department entity);
        Department TGetById(int id);
    }
}
