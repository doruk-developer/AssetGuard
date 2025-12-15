namespace AssetGuard.Entity;

public partial class Assignment
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int AssetId { get; set; }

    public DateTime AssignedDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Notes { get; set; }

    public virtual Asset Asset { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
