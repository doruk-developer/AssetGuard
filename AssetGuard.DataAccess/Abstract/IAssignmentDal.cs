// Tanım: Assignment varlığı için veritabanında yapılabilecek tüm metodların (Add, Update, Delete) listesidir.
using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IAssignmentDal
    {
        void Add(Assignment entity);
        List<Assignment> GetAllWithDetails();
    }
}
