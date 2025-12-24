using AssetGuard.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetGuard.DataAccess.Context
{
    public class ZimmetContext : IdentityDbContext<AppUser>
    {
        // --- KRİTİK CONSTRUCTOR ---
        // Program.cs'deki "options.UseSqlServer" ayarını içeri alan kapı burasıdır.
        public ZimmetContext(DbContextOptions<ZimmetContext> options) : base(options)
        {
        }

        // Tablolar
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetStatus> AssetStatuses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        
        // Raporlama için View veya Tablo varsa buraya eklenir, yoksa kalabilir.

        // Bağlantı adresi artık dışarıdan geldiği için burayı boş bırakıyoruz.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Buraya kod yazmana gerek yok.
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity tabloları için gerekli
            base.OnModelCreating(builder);

            // Tablo İsimlerini Sabitleme
            builder.Entity<Asset>().ToTable("Assets");
            builder.Entity<AssetStatus>().ToTable("AssetStatus");
            builder.Entity<Assignment>().ToTable("Assignments");
            builder.Entity<Category>().ToTable("Categories");
            builder.Entity<Department>().ToTable("Departments");
            builder.Entity<Employee>().ToTable("Employees");

            // Özel Ayarlar (Fluent API)
            builder.Entity<Asset>(entity =>
            {
                // Seri No benzersiz olsun
                entity.HasIndex(e => e.SerialNo).IsUnique(); 
                // Fiyat hassasiyeti
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)"); 
            });
        }
    }
}