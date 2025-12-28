namespace AssetGuard.Entity;

using AssetGuard.Entity.Base;

public class Assignment : BaseEntity // BaseEntity sınıfından miras al
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int AssetId { get; set; }

    public DateTime AssignmentDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Notes { get; set; }

    public virtual Asset Asset { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
