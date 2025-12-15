using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class AssetManager : IAssetService
    {
        // Business katmanı DataAccess katmanını kullanır (Bağımlılık)
        private readonly IAssetDal _assetDal;

        public AssetManager(IAssetDal assetDal)
        {
            _assetDal = assetDal;
        }

        public void TAdd(Asset entity)
        {
            // İleride buraya iş kuralları yazacağız (Örn: Fiyat negatif olamaz)
            _assetDal.Add(entity);
        }

        public void TDelete(Asset entity)
        {
            _assetDal.Delete(entity);
        }

        public List<Asset> TGetAll()
        {
            return _assetDal.GetAll();
        }

        public Asset TGetById(int id)
        {
            return _assetDal.GetById(id);
        }

        public void TUpdate(Asset entity)
        {
            _assetDal.Update(entity);
        }

        // Dashboard Verilerini Taşıyoruz
        public int TGetTotalAssetCount()
        {
            return _assetDal.GetTotalAssetCount();
        }

        public decimal TGetTotalInventoryValue()
        {
            return _assetDal.GetTotalInventoryValue();
        }
    }
}