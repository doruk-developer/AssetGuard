using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface ICategoryDal
    {
        List<Category> GetAll();
        // --- EKLENEN CRUD İMZALARI ---
        void Add(Category entity);
        void Delete(Category entity);
        void Update(Category entity);
        Category GetById(int id);
    }
}
