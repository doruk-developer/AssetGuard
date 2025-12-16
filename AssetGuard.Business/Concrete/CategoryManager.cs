using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal) { _categoryDal = categoryDal; }

        public List<Category> TGetAll() { return _categoryDal.GetAll(); }


        public void TAdd(Category entity) { _categoryDal.Add(entity); }
        public void TDelete(Category entity) { _categoryDal.Delete(entity); }
        public void TUpdate(Category entity) { _categoryDal.Update(entity); }
        public Category TGetById(int id) { return _categoryDal.GetById(id); }
    }
}
