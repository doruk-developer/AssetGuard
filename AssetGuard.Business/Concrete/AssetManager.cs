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

        // 1. Garanti Raporu Sorgusu için
        public List<Asset> TGetAssetsExpiringSoon(int days)
        {
            var expiryDate = DateTime.Now.AddDays(days);

            // Not: _assetDal.GetAll() metodunun "Include" (ilişkili verileri getirme) 
            // yaptığından emin olmalıyız (Önceki adımlarda yapmıştık).
            return _assetDal.GetAll()
                .Where(x => x.WarrantyEndDate != null && x.WarrantyEndDate <= expiryDate && x.WarrantyEndDate >= DateTime.Now)
                .ToList();
        }

        // 2. Arıza Raporu Sorgusu için
        public List<Asset> TGetAssetsByStatus(string statusName)
        {
            return _assetDal.GetAll()
                .Where(x => x.Status != null && x.Status.Name.Contains(statusName))
                .ToList();
        }

        // 3. Mali Durum Raporu Sorgusu için
        public List<Asset> TGetAllWithDetails()
        {
            // Zaten DAL katmanındaki GetAll metodu Include ile dolu getiriyordu.
            return _assetDal.GetAll();
        }

    }
}