using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Concrete
{
    public class EfAssetStatusDal : IAssetStatusDal
    {
        private readonly ZimmetContext _context;
        public EfAssetStatusDal(ZimmetContext context) { _context = context; }

        public List<AssetStatus> GetAll() { return _context.AssetStatuses.ToList(); }

        public void Add(AssetStatus entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(AssetStatus entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }
        public void Update(AssetStatus entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
        public AssetStatus GetById(int id)
        {
            return _context.AssetStatuses.Find(id);
        }
    }
}
