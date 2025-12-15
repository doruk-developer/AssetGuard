// --- N-Tier Baðlantýlarýný Kurma (Dependency Injection) ---

// 1. Veritabaný Baðlantýsý (Context)
using AssetGuard.Business.Abstract;
using AssetGuard.Business.Concrete;
using AssetGuard.DataAccess.Abstract;
using AssetGuard.DataAccess.Concrete;
using AssetGuard.DataAccess.Context;
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

// Departman Servisleri
builder.Services.AddScoped<IDepartmentService, DepartmentManager>();
builder.Services.AddScoped<IDepartmentDal, EfDepartmentDal>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
