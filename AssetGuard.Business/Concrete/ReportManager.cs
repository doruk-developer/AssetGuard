using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity.Reports;

namespace AssetGuard.Business.Concrete
{
    public class ReportManager : IReportService
    {
        private readonly IReportDal _reportDal;
        public ReportManager(IReportDal reportDal) { _reportDal = reportDal; }

        public List<AssetDistributionReport> GetAssetDistributionReport()
        {
            return _reportDal.GetAssetDistribution();
        }

        public List<MonthlyExpenseReport> GetMonthlyExpenseReport()
        {
            return _reportDal.GetMonthlyExpenses();
        }

    }
}
