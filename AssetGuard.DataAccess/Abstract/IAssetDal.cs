using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IAssetDal
    {
        // Standart CRUD işlemleri
        void Add(Asset entity);
        void Delete(Asset entity);
        void Update(Asset entity);
        List<Asset> GetAll();
        Asset GetById(int id);

        // Dashboard için özel metotlar
        decimal GetTotalInventoryValue(); // Toplam Para
        int GetTotalAssetCount();         // Toplam Adet
    }
}