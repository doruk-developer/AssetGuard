// Tanım: IAssignmentDal arayüzündeki metodları (Add) Entity Framework kullanarak SQL'e yansıtan somut sınıftır.

using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;
using Microsoft.EntityFrameworkCore; // Include için şart

namespace AssetGuard.DataAccess.Concrete
{
    public class EfAssignmentDal : IAssignmentDal
    {
        private readonly ZimmetContext _context;
        public EfAssignmentDal(ZimmetContext context) { _context = context; }

        public void Add(Assignment entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public List<Assignment> GetAllWithDetails()
        {
            // JOIN İŞLEMİ: Zimmet kaydını çekerken yanına Ürün (Asset) 
            // ve Personel (Employee) bilgilerini de alıp getirir.
            return _context.Assignments
                .Include(x => x.Asset)
                .Include(x => x.Employee)
                .OrderByDescending(x => x.AssignmentDate) // En son yapılan zimmet en üstte
                .ToList();
        }
    }
}