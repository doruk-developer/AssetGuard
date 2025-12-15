using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Concrete
{
    public class EfEmployeeDal : IEmployeeDal
    {
        private readonly ZimmetContext _context;
        public EfEmployeeDal(ZimmetContext context) { _context = context; }

        public List<Employee> GetAll() { return _context.Employees.ToList(); }
        public Employee GetById(int id) { return _context.Employees.Find(id); }
    }
}
