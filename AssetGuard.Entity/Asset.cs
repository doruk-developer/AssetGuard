using AssetGuard.Entity.Base;

namespace AssetGuard.Entity;

public partial class Asset : BaseEntity
{
    // public int Id { get; set; } artık BaseEntity tarafından sağlanıyor

    public string AssetName { get; set; } = null!;

    public string SerialNo { get; set; } = null!;

    public int CategoryId { get; set; }

    public int StatusId { get; set; }

    public decimal Price { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? WarrantyEndDate { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual Category Category { get; set; } = null!;

    public virtual AssetStatus Status { get; set; } = null!;
}
