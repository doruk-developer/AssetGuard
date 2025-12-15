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
    }
}
