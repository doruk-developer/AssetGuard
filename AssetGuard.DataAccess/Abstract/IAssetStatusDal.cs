using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IAssetStatusDal
    {
        List<AssetStatus> GetAll();
        // --- Demirbaş Durumları CRUD işlemleri için ---
        void Add(AssetStatus entity);
        void Delete(AssetStatus entity);
        void Update(AssetStatus entity);
        AssetStatus GetById(int id);
    }
}
