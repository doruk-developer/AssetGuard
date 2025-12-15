using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IAssetStatusDal
    {
        List<AssetStatus> GetAll();
    }
}
