using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Concrete
{
    public class EfCategoryDal : ICategoryDal
    {
        private readonly ZimmetContext _context;
        public EfCategoryDal(ZimmetContext context) { _context = context; }

        public List<Category> GetAll() { return _context.Categories.ToList(); }

        public void Add(Category entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Category entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
        public void Update(Category entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }
    }
}
