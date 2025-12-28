using AssetGuard.Entity.Base;

namespace AssetGuard.Entity;

public class Employee : BaseEntity // BaseEntity sınıfından miras al
{
    // public int Id { get; set; } artık BaseEntity tarafından sağlanıyor

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Department Department { get; set; } = null!;
    public string Email { get; set; }
}
