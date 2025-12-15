// Tanım: Business kurallarını uygulayan ve DAL (Veri Erişim) metodlarını çağıran yönetici sınıftır.

using AssetGuard.Business.Abstract; // <-- IAssigmnetentService'le uyumlu çalışma için gerekli
using AssetGuard.DataAccess.Abstract;
using AssetGuard.Entity;

namespace AssetGuard.Business.Concrete
{
    public class AssignmentManager : IAssignmentService
    {
        private readonly IAssignmentDal _assignmentDal;

        public AssignmentManager(IAssignmentDal assignmentDal)
        {
            _assignmentDal = assignmentDal;
        }

        public void TAdd(Assignment entity)
        {
            _assignmentDal.Add(entity);
        }

        public List<Assignment> TGetAllWithDetails()
        {
            // Kuralı uyguluyoruz: Veri çekme işini DAL (DataAccess) katmanına devret
            return _assignmentDal.GetAllWithDetails();
        }
    }
}
