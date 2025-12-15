using AssetGuard.Entity;

namespace AssetGuard.DataAccess.Abstract
{
    public interface IEmployeeDal
    {
        List<Employee> GetAll();
        Employee GetById(int id);
    }
}
