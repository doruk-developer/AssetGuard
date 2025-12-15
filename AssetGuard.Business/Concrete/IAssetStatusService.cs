using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IAssetStatusService
    {
        List<AssetStatus> TGetAll();
    }
}
