using AssetGuard.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AssetGuard.DataAccess.Context
{
    // IdentityDbContext<AppUser> olmalı!
    public partial class ZimmetContext : IdentityDbContext<AppUser>
    {
        public ZimmetContext()
        {
        }

        public ZimmetContext(DbContextOptions<ZimmetContext> options)
            : base(options)
        {
        }

        // Mevcut Tablolar
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetStatus> AssetStatuses { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Bağlantı adresi (Senin bilgisayarın için doğru olan)
                optionsBuilder.UseSqlServer("Server=WORK-COMPUTER\\SQLDEV_2022;Database=ZimmetDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // !!! KRİTİK NOKTA !!!
            // Bu satır Identity (Login) tablolarını oluşturur. En başta olmalı.
            base.OnModelCreating(modelBuilder);

            // Tablo isimlerini sabitleme (ToTable)
            modelBuilder.Entity<AssetStatus>().ToTable("AssetStatus");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Assignment>().ToTable("Assignments");
            modelBuilder.Entity<Asset>().ToTable("Assets");

            // Mevcut Fluent API kodların...
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Assets__3214EC27DA32BCBB");
                entity.HasIndex(e => e.SerialNo, "UQ__Assets__SerialNo").IsUnique();
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AssetName).HasMaxLength(100);
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.ImageUrl).HasMaxLength(250);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PurchaseDate).HasDefaultValueSql("(getdate())");
                // entity.Property(e => e.ReturnDate) <-- Bunu silmiştik, hata veriyordu.
                entity.Property(e => e.SerialNo).HasMaxLength(50);
                entity.Property(e => e.StatusId).HasColumnName("StatusID");
                entity.Property(e => e.WarrantyEndDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category).WithMany(p => p.Assets)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assets_Categories");

                entity.HasOne(d => d.Status).WithMany(p => p.Assets)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Assets_Status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}