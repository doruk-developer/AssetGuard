using AssetGuard.Entity.Reports;

namespace AssetGuard.Business.Abstract
{
    public interface IReportService
    {
        List<AssetDistributionReport> GetAssetDistributionReport();
    }
}
