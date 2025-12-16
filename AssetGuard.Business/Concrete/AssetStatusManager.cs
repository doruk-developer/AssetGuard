using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class AssetStatusManager : IAssetStatusService
    {
        private readonly IAssetStatusDal _assetStatusDal;
        public AssetStatusManager(IAssetStatusDal assetStatusDal) { _assetStatusDal = assetStatusDal; }

        public List<AssetStatus> TGetAll() { return _assetStatusDal.GetAll(); }

        public void TAdd(AssetStatus entity) { _assetStatusDal.Add(entity); }
        public void TDelete(AssetStatus entity) { _assetStatusDal.Delete(entity); }
        public void TUpdate(AssetStatus entity) { _assetStatusDal.Update(entity); }
        public AssetStatus TGetById(int id) { return _assetStatusDal.GetById(id); }
    }
}