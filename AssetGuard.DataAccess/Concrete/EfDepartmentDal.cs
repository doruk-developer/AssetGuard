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
    }
}
