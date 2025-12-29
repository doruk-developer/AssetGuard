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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ALTYAPI SERVÝSLERÝ ---
// Audit Log (Ýz Kayýtlarý) için "Kim giriþ yaptý?" bilgisini Context'e taþýr.
builder.Services.AddHttpContextAccessor();

// =========================================================
// 2. AKILLI BAÐLANTI SEÇÝCÝ (AUTO-DISCOVERY v3)
// =========================================================
string? validConnectionString = null;
var connectionStrings = builder.Configuration.GetSection("ConnectionStrings").GetChildren();

Console.WriteLine(">>> Veritabaný baðlantýsý aranýyor...");

foreach (var conn in connectionStrings)
{
	string? testConnString = conn.Value;
	if (string.IsNullOrEmpty(testConnString)) continue;

	try
	{
		// 3 saniyelik hýzlý test: Makinalar arasý isim çözme gecikmelerini kapsar.
		using (var connection = new SqlConnection(testConnString))
		{
			var builderConn = new SqlConnectionStringBuilder(testConnString) { ConnectTimeout = 3 };
			connection.ConnectionString = builderConn.ConnectionString;
			connection.Open();
			validConnectionString = testConnString;
			Console.WriteLine($">>> BAÞARILI! Baðlanýlan Sunucu: {conn.Key}");
			break;
		}
	}
	catch
	{
		Console.WriteLine($"--- Atlandý: {conn.Key}");
		continue;
	}
}

// Hiçbir özel baðlantý çalýþmazsa DefaultConnection'a (Local) düþer.
if (string.IsNullOrEmpty(validConnectionString))
{
	validConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	Console.WriteLine(">>> UYARI: Özel sunucular bulunamadý. Varsayýlan ayar kullanýlýyor.");
}

builder.Services.AddDbContext<ZimmetContext>(options => options.UseSqlServer(validConnectionString));
// =========================================================

// --- 3. DEPENDENCY INJECTION (Katmanlý Mimari Kayýtlarý) ---
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

// --- 4. IDENTITY & GÜVENLÝK ---
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

builder.Services.AddValidatorsFromAssemblyContaining<AssetGuard.Business.ValidationRules.AssetValidator>();

var app = builder.Build();

// --- 5. ARA KATMAN (Middleware Pipeline) ---
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error/Page500");
}
app.UseStatusCodePagesWithReExecute("/Error/Page404", "?code={0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

// --- 6. OTOMATÝK KURULUM MOTORU (Seed Data) ---
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		var context = services.GetRequiredService<ZimmetContext>();
		var userManager = services.GetRequiredService<UserManager<AppUser>>();
		var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

		// Veritabanýný otomatik oluþturur (Eksikse)
		context.Database.Migrate();

		// Rolleri Tanýmla
		if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));
		if (!await roleManager.RoleExistsAsync("User")) await roleManager.CreateAsync(new IdentityRole("User"));

		// Admin Kullanýcýsý
		var adminUser = await userManager.FindByNameAsync("admin");
		if (adminUser == null)
		{
			adminUser = new AppUser { UserName = "admin", Email = "admin@assetguard.com", FirstName = "Sistem", LastName = "Yöneticisi", EmailConfirmed = true };
			await userManager.CreateAsync(adminUser, "123");
			await userManager.AddToRoleAsync(adminUser, "Admin");
		}

		// Standart Kullanýcý
		var normalUser = await userManager.FindByNameAsync("user");
		if (normalUser == null)
		{
			normalUser = new AppUser { UserName = "user", Email = "user@assetguard.com", FirstName = "Personel", LastName = "Kullanýcýsý", EmailConfirmed = true };
			await userManager.CreateAsync(normalUser, "321");
			await userManager.AddToRoleAsync(normalUser, "User");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine(">>> SÝSTEM KURULUM HATASI: " + ex.Message);
	}
}

// --- 7. UYGULAMAYI BAÞLAT ---
app.Run();