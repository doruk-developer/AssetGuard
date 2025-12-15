using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface ICategoryDal
    {
        List<Category> GetAll();
    }
}
