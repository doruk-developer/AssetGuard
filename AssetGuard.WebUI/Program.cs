// --- N-Tier Baðlantýlarýný Kurma (Dependency Injection) ---

// 1. Veritabaný Baðlantýsý (Context)
using AssetGuard.Business.Abstract;
using AssetGuard.Business.Concrete;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Concrete;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient; // Baðlantý testi için gerekli
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// 1. AKILLI BAÐLANTI SEÇÝCÝ (AUTO-DISCOVERY)
// =========================================================
string validConnectionString = null;
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings").GetChildren();

Console.WriteLine(">>> Veritabaný baðlantýsý aranýyor...");

foreach (var conn in connectionStrings)
{
    string testConnString = conn.Value;
    try
    {
        // 1 saniyelik hýzlý bir baðlantý testi yapýyoruz
        using (var connection = new SqlConnection(testConnString))
        {
            // Baðlantý zaman aþýmýný kýsa tutuyoruz ki bekletmesin
            var builderConn = new SqlConnectionStringBuilder(testConnString) { ConnectTimeout = 2 };
            connection.ConnectionString = builderConn.ConnectionString;

            connection.Open(); // Baðlanmayý dene
            validConnectionString = testConnString; // Baþarýlýysa bunu seç
            Console.WriteLine($">>> BAÞARILI! Baðlanýlan Sunucu: {conn.Key}");
            break; // Döngüden çýk
        }
    }
    catch
    {
        Console.WriteLine($"--- Baþarýsýz: {conn.Key} (Sunucu yok veya ulaþýlamýyor)");
        continue; // Sýradakini dene
    }
}

if (string.IsNullOrEmpty(validConnectionString))
{
    // Hiçbiri çalýþmazsa en güvenli liman LocalDB'ye veya Default'a düþ
    validConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine(">>> UYARI: Hiçbir özel sunucu bulunamadý. Varsayýlan ayar kullanýlýyor.");
}

// Seçilen çalýþan adresi Context'e veriyoruz
builder.Services.AddDbContext<ZimmetContext>(options => options.UseSqlServer(validConnectionString));
// =========================================================


// --- 2. DEPENDENCY INJECTION (DI) ---
builder.Services.AddScoped<IAssetService, AssetManager>();
builder.Services.AddScoped<IAssetDal, EfAssetDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<IAssetStatusService, AssetStatusManager>();
builder.Services.AddScoped<IAssetStatusDal, EfAssetStatusDal>();
builder.Services.AddScoped<IEmployeeService, EmployeeManager>();
builder.Services.AddScoped<IEmployeeDal, EfEmployeeDal>();
builder.Services.AddScoped<IAssignmentService, AssignmentManager>();
builder.Services.AddScoped<IAssignmentDal, EfAssignmentDal>();
builder.Services.AddScoped<IDepartmentService, DepartmentManager>();
builder.Services.AddScoped<IDepartmentDal, EfDepartmentDal>();
builder.Services.AddScoped<IReportService, ReportManager>();
builder.Services.AddScoped<IReportDal, EfReportDal>();
builder.Services.AddScoped<IEmailService, EmailManager>();

// --- 3. IDENTITY ---
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<ZimmetContext>()
.AddDefaultTokenProviders();

// --- 4. GÜVENLÝK AYARLARI ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// --- 5. VALIDASYON ---
builder.Services.AddValidatorsFromAssemblyContaining<AssetGuard.Business.ValidationRules.AssetValidator>();

var app = builder.Build();

// --- 6. HATA YÖNETÝMÝ ---
// Geliþtirme ortamýnda bile olsak gerçek hata sayfalarýný test etmek için:
app.UseExceptionHandler("/Error/Page500");
app.UseStatusCodePagesWithReExecute("/Error/Page404", "?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- 7. OTOMATÝK KURULUM MOTORU (SEED DATA) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ZimmetContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        // Veritabaný yoksa oluþtur (Migrationlarý bas)
        // NOT: Bu iþlem, yukarýda seçilen 'validConnectionString' adresine yapýlýr.
        context.Database.Migrate();

        // Otomatik Admin
        var adminUser = userManager.FindByNameAsync("admin").GetAwaiter().GetResult();
        if (adminUser == null)
        {
            var newAdmin = new AppUser
            {
                UserName = "admin",
                Email = "admin@assetguard.com",
                FirstName = "Sistem",
                LastName = "Yöneticisi",
                EmailConfirmed = true
            };
            userManager.CreateAsync(newAdmin, "123").GetAwaiter().GetResult();
        }
    }
    catch (Exception ex)
    {
        // Eðer veritabaný baðlantýsý veya oluþturma sýrasýnda hata olursa konsola yaz
        Console.WriteLine(">>> KRÝTÝK HATA (DB Init): " + ex.Message);
    }
}

// --- 8. OTOMATÝK KURULUM VE ROL/KULLANICI MOTORU (SEED DATA) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ZimmetContext>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); // Rol Yöneticisi

        // A) Veritabaný yoksa oluþtur (Migration)
        //context.Database.Migrate();

        // B) ROLLERÝ OLUÞTUR (Yoksa Ekle)
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));

        // C) 1. KULLANICI: ADMIN (Tam Yetki) -> Þifre: 123
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@assetguard.com",
                FirstName = "Sistem",
                LastName = "Yöneticisi",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin"); // Rütbeyi tak
            }
        }
        else
        {
            // Eðer kullanýcý zaten varsa ama rolü yoksa, rolü ekle (Eski veriyi düzeltmek için)
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // D) 2. KULLANICI: USER (Kýsýtlý Yetki) -> Þifre: 321
        var normalUser = await userManager.FindByNameAsync("user");
        if (normalUser == null)
        {
            normalUser = new AppUser
            {
                UserName = "user",
                Email = "user@assetguard.com",
                FirstName = "Personel",
                LastName = "Kullanýcýsý",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(normalUser, "321");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, "User");
                Console.WriteLine(">>> User kullanýcýsý BAÞARIYLA oluþturuldu.");
            }
            else
            {
                // HATA VARSA YAZDIR (Dedektör)
                Console.WriteLine("!!! USER OLUÞTURULAMADI. SEBEPLER:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
            }
        }
    }


    catch (Exception ex)
    {
        // Eðer veritabaný baðlantýsý veya oluþturma sýrasýnda hata olursa konsola yaz
        Console.WriteLine(">>> KRÝTÝK HATA (SEED DATA): " + ex.Message);
    }
    app.Run();
}