using AssetGuard.Entity.Base;

namespace AssetGuard.Entity;

public class Category : BaseEntity
{
    // public int Id { get; set; } artık BaseEntity tarafından sağlanıyor

    public string Name { get; set; } = null!;

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
