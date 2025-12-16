// Tanım: IDepartmentService implementasyonu ile DAL'dan çekilen veriyi UI katmanına
// hazırlayıp sunan iş mantığı sınıfıdır.

using AssetGuard.Business.Abstract;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class DepartmentManager : IDepartmentService
    {
        private readonly IDepartmentDal _departmentDal;
        public DepartmentManager(IDepartmentDal departmentDal) { _departmentDal = departmentDal; }

        public List<Department> TGetAll() { return _departmentDal.GetAll(); }

        // --- Departman CRUD İşlemleri İçin ---
        public void TAdd(Department entity) { _departmentDal.Add(entity); }
        public void TDelete(Department entity) { _departmentDal.Delete(entity); }
        public void TUpdate(Department entity) { _departmentDal.Update(entity); }
        public Department TGetById(int id) { return _departmentDal.GetById(id); }
    }
}
