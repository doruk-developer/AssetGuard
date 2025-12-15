// Tanım: Employee Entity'si için tüm CRUD ve sorgulama operasyonlarının metod imzalarını tanımlar.

using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;
using Microsoft.EntityFrameworkCore; // Include için şart

namespace AssetGuard.DataAccess.Concrete
{
    public class EfEmployeeDal : IEmployeeDal
    {
        private readonly ZimmetContext _context;
        public EfEmployeeDal(ZimmetContext context) { _context = context; }

        // EKLEME
        public void Add(Employee entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        // SİLME
        public void Delete(Employee entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        // GÜNCELLEME
        public void Update(Employee entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        // GET BY ID
        public Employee GetById(int id)
        {
            return _context.Employees.Find(id);
        }

        // LİSTELEME (Departman Dahil)
        public List<Employee> GetAll()
        {
            return _context.Employees
                .Include(x => x.Department) // Departman adını çekmek için
                .ToList();
        }
    }
}