// Tanım: IDepartmentDal arayüz implementasyonunu Entity Framework Core
//   ile gerçekleştiren somut veri erişim sınıfıdır.

using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Concrete
{
    public class EfDepartmentDal : IDepartmentDal
    {
        private readonly ZimmetContext _context;
        public EfDepartmentDal(ZimmetContext context) { _context = context; }

        public List<Department> GetAll() { return _context.Departments.ToList(); }

        // --- YENİ EKLENENLER ---
        public void Add(Department entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Department entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
        public void Update(Department entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
        public Department GetById(int id)
        {
            return _context.Departments.Find(id);
        }
    }
}
