using Microsoft.AspNetCore.Identity;

namespace AssetGuard.Entity;

/// <summary>
/// Uygulama kullanıcılarını temsil eden genişletilmiş Identity sınıfı.
/// IdentityUser'dan miras alarak Username, Email, PasswordHash gibi temel alanları hazır getirir.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// Kullanıcının gerçek adı.
    /// </summary>
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Kullanıcının gerçek soyadı.
    /// </summary>
    public string LastName { get; set; } = null!;

    // Yeni eklenen alan: Tüm tema tercihlerini JSON formatında saklayacak.
    public string? ThemeSettingsJson { get; set; }

}
