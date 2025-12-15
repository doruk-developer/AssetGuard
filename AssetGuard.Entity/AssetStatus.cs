using System.ComponentModel.DataAnnotations.Schema; // Bu kütüphane şart

namespace AssetGuard.Entity
{
    [Table("AssetStatus")] // <-- İŞTE KİLİT NOKTA: Tablo adını sabitliyoruz.
    public partial class AssetStatus
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
