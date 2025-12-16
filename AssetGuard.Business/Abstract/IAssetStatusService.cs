using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IAssetStatusService
    {
        // 1. Listeleme
        List<AssetStatus> TGetAll();

        // 2. Ekleme
        void TAdd(AssetStatus entity);

        // 3. Silme
        void TDelete(AssetStatus entity);

        // 4. Güncelleme
        void TUpdate(AssetStatus entity);

        // 5. ID'ye göre getirme
        AssetStatus TGetById(int id);
    }
}
