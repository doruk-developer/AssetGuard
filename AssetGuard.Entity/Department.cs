namespace AssetGuard.Entity;
using AssetGuard.Entity.Base;

public class Department : BaseEntity // BaseEntity sınıfından miras al
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
