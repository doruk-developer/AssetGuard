// --- N-Tier Baðlantýlarýný Kurma (Dependency Injection) ---

// 1. Veritabaný Baðlantýsý (Context)
using AssetGuard.Business.Abstract;
using AssetGuard.Business.Concrete;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Concrete;
using AssetGuard.DataAccess.Context;
using AssetGuard.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // ÖNCE BUILDER OLUÞTURULUR

// --- N-Tier Baðlantýlarýný Kurma (Dependency Injection) ---
// BU KODLAR BUILDER OLUÞTURULDUKTAN SONRA GELMELÝDÝR
// Çünkü builder.Services'a eriþim/emir verebilmek için önce builder'ýn tanýmlanmasý gerekir. 

// 1. Veritabaný Baðlantýsý (Context)
builder.Services.AddDbContext<ZimmetContext>(options =>
{
    options.UseSqlServer("Server=WORK-COMPUTER\\SQLDEV_2022;Database=ZimmetDB;Trusted_Connection=True;TrustServerCertificate=True;");
});

// 2. Business ve DataAccess Servislerini Kaydetme
builder.Services.AddScoped<IAssetService, AssetManager>();
builder.Services.AddScoped<IAssetDal, EfAssetDal>();

// --- Lookup Servisleri ---
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

builder.Services.AddScoped<IAssetStatusService, AssetStatusManager>();
builder.Services.AddScoped<IAssetStatusDal, EfAssetStatusDal>();

// --- Employee-Zimmetleme Servisleri ---
builder.Services.AddScoped<IEmployeeService, EmployeeManager>();
builder.Services.AddScoped<IEmployeeDal, EfEmployeeDal>();

// --- Zimmet Ata Servisleri ---
builder.Services.AddScoped<IAssignmentService, AssignmentManager>();
builder.Services.AddScoped<IAssignmentDal, EfAssignmentDal>();

// --- Departman Servisleri ---
builder.Services.AddScoped<IDepartmentService, DepartmentManager>();
builder.Services.AddScoped<IDepartmentDal, EfDepartmentDal>();

// --- Rapor Servisleri ---
builder.Services.AddScoped<IReportService, ReportManager>();
builder.Services.AddScoped<IReportDal, EfReportDal>();

// --- Kullanýcý Servisleri ---
builder.Services.AddScoped<IEmailService, EmailManager>();

// --- 1. IDENTITY SERVISLERINI EKLE ---
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Parola kurallarýný þimdilik esnek tutalým (Geliþtirme aþamasý)
    options.Password.RequiredLength = 3; // En az 3 karakter
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<ZimmetContext>()
.AddDefaultTokenProviders();

// --- 2. LOGIN YÖNLENDÝRME AYARI ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Giriþ yapmayan buraya gider
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Sistemin her yerini otomatik kilitleyen Global Filtre
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build(); // EN SONRA BUILD EDÝLÝR


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication(); // Kimlik Doðrulama (Ben kimim?)
app.UseAuthorization();  // Yetkilendirme (Nereye girebilirim?)

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// --- OTOMATÝK ADMÝN OLUÞTURMA (SEED USER) ---
// Uygulama her baþladýðýnda çalýþýr, admin yoksa ekler.
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<AssetGuard.Entity.AppUser>>();

    // Admin kullanýcýsý var mý diye bak
    var adminUser = userManager.FindByNameAsync("admin").Result;

    if (adminUser == null)
    {
        // Yoksa oluþtur
        adminUser = new AssetGuard.Entity.AppUser
        {
            UserName = "admin",
            Email = "admin@assetguard.com",
            FirstName = "Sistem",
            LastName = "Yöneticisi",
            EmailConfirmed = true
        };

        // Þifreyi (123) Identity sistemiyle güvenli þekilde oluþturup kaydet
        var result = userManager.CreateAsync(adminUser, "123").Result;

        if (result.Succeeded)
        {
            Console.WriteLine(">>> Admin kullanýcýsý (admin/123) baþarýyla oluþturuldu.");
        }
    }
}

app.Run();
