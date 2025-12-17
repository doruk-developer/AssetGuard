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

        public List<MonthlyExpenseReport> GetMonthlyExpenses()
        {
            var data = _context.Assets
                .Where(x => x.PurchaseDate > DateTime.Now.AddYears(-5)) // DEĞİŞTİ: 6 ay yerine 5 Yıl yaptık
                .AsEnumerable()
                .GroupBy(x => new { x.PurchaseDate.Year, x.PurchaseDate.Month })
                .Select(g => new MonthlyExpenseReport
                {
                    // Hem Yılı hem Ayı gösterelim ki karışmasın (Örn: Ocak 2024)
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    TotalAmount = g.Sum(x => x.Price)
                })
                .ToList();

            return data;
        }

    }
}
