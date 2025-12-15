using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;
using Microsoft.EntityFrameworkCore;

namespace AssetGuard.DataAccess.Concrete
{
    public class EfAssetDal : IAssetDal
    {
        private readonly ZimmetContext _context;

        // Dependency Injection ile Context'i alıyoruz
        public EfAssetDal(ZimmetContext context)
        {
            _context = context;
        }

        public void Add(Asset entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Asset entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public List<Asset> GetAll()
        {
            // ARTIK GÜVENLE AÇABİLİRİZ
            // ToTable ayarı yapıldığı için bu satırlar hata vermeyecek.
            return _context.Assets
                .Include(x => x.Category)  // <-- Eager Loading include'u (Kategori ismini getir)
                .Include(x => x.Status)    // <-- Eager Loading include'u (Durum ismini getir)
                .ToList();
        }

        public Asset GetById(int id)
        {
            return _context.Assets.Find(id);
        }

        public void Update(Asset entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        // --- DASHBOARD ÖZEL METOTLARI ---

        public int GetTotalAssetCount()
        {
            // Veritabanındaki toplam kayıt sayısı
            return _context.Assets.Count();
        }

        public decimal GetTotalInventoryValue()
        {
            // Tüm ürünlerin (Price) kolonunu topla
            return _context.Assets.Sum(x => x.Price);
        }
    }
}