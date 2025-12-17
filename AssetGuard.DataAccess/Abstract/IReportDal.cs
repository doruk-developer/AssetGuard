using AssetGuard.Entity.Reports;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IReportDal
    {
        // Kategori dağılım raporunu getiren metot
        List<AssetDistributionReport> GetAssetDistribution();
        List<MonthlyExpenseReport> GetMonthlyExpenses();
    }
}