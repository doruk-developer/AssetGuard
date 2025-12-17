using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity.Reports;
using Microsoft.EntityFrameworkCore;

namespace AssetGuard.DataAccess.Concrete
{
    public class EfReportDal : IReportDal
    {
        private readonly ZimmetContext _context;
        public EfReportDal(ZimmetContext context) { _context = context; }

        public List<AssetDistributionReport> GetAssetDistribution()
        {
            // SQL'deki GROUP BY işleminin C# hali:
            // 1. Kategorileri dahil et
            // 2. Kategori adına göre grupla
            // 3. Her grubun sayısını ve fiyat toplamını al
            return _context.Assets
                .Include(x => x.Category)
                .GroupBy(x => x.Category.Name)
                .Select(g => new AssetDistributionReport
                {
                    CategoryName = g.Key,
                    AssetCount = g.Count(),
                    TotalPrice = g.Sum(x => x.Price)
                })
                .ToList();
        }
    }
}
