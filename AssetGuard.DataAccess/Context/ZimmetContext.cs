using AssetGuard.Entity;
using AssetGuard.Entity.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetGuard.DataAccess.Context
{
    public class ZimmetContext : IdentityDbContext<AppUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ZimmetContext(DbContextOptions<ZimmetContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetStatus> AssetStatuses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        // --- YÖNTEM 1: SENKRON KAYIT (SaveChanges) ---
        // Senin DAL katmanın muhtemelen bunu kullanıyor.
        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        // --- YÖNTEM 2: ASENKRON KAYIT (SaveChangesAsync) ---
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        // --- ORTAK MANTIK (DRY - Don't Repeat Yourself) ---
        private void SetAuditFields()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistem";
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = userName;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        // ModifiedDate'i şu anki zaman yap
                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.ModifiedBy = userName;

                        // CreatedDate'i koru (Veritabanından gelen neyse o kalsın, değiştirme)
                        entry.Property(x => x.CreatedDate).IsModified = false;
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        break;

                    case EntityState.Deleted:
                        // Soft Delete
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.ModifiedDate = DateTime.Now;
                        entry.Entity.ModifiedBy = userName;
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Asset>().ToTable("Assets");
            builder.Entity<AssetStatus>().ToTable("AssetStatus");
            builder.Entity<Assignment>().ToTable("Assignments");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Department>().ToTable("Departments");
            builder.Entity<Employee>().ToTable("Employees");

            // Global Filtre (Soft Delete Olanları Getirme)
            builder.Entity<Asset>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Department>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Employee>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<Assignment>().HasQueryFilter(x => !x.IsDeleted);
            builder.Entity<AssetStatus>().HasQueryFilter(x => !x.IsDeleted);

            builder.Entity<Asset>(entity =>
            {
                entity.HasIndex(e => e.SerialNo).IsUnique();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
        }
    }
}