// Tanım: Department verilerinin iş katmanı seviyesinde sunulması için
// gerekli servis metod imzalarını barındırır.

using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IDepartmentService
    {
        List<Department> TGetAll();
    }
}
