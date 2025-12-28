// Soft Delete ve Audit Log için oluştırulmuş temel entity sınıfı.

namespace AssetGuard.Entity.Base
{
    public abstract class BaseEntity
    {
        public int Id { get; set; } // Tüm tablolarda ID ortaktır

        // --- İZ KAYITLARI (AUDIT LOGS) ---
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Oluşturulma Tarihi
        public string? CreatedBy { get; set; } // Kim oluşturdu?

        public DateTime? ModifiedDate { get; set; } // Güncellenme Tarihi
        public string? ModifiedBy { get; set; } // Kim güncelledi?

        // --- YUMUŞAK SİLME (SOFT DELETE) ---
        public bool IsDeleted { get; set; } = false; // Silindi mi?
    }
}