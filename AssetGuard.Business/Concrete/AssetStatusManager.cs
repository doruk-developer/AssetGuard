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
    }
}