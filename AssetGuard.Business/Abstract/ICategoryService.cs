using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface ICategoryService
    {
        List<Category> TGetAll();
        // --- EKLENENLER ---
        void TAdd(Category entity);
        void TDelete(Category entity);
        void TUpdate(Category entity);
        Category TGetById(int id);
    }
}
