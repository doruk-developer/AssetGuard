// Tanım: Assignment işlemleri için Business (İş Kuralı) katmanında bulunacak tüm public metodların listesidir.

using AssetGuard.Entity;

namespace AssetGuard.Business.Abstract
{
    public interface IAssignmentService
    {
        // Manager sınıfında bu metodun AYNISI olmak zorunda
        void TAdd(Assignment entity);
        List<Assignment> TGetAllWithDetails();
    }
}
