using AssetGuard.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema; // Bu kütüphane şart

namespace AssetGuard.Entity
{
    [Table("AssetStatus")] // <-- İŞTE KİLİT NOKTA: Tablo adını sabitliyoruz.
    public class AssetStatus : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        // --- Dinamik Renk Sistemi için ---
        public string? ColorClass { get; set; } // Bootstrap renk kodu (success, danger vs.)

        public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
