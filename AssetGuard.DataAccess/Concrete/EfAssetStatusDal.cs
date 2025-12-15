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
    }
}
