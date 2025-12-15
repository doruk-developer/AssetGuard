using AssetGuard.Entity; // Yeni adres
using Microsoft.EntityFrameworkCore;

namespace AssetGuard.DataAccess.Context // Yeni, doğru adres
{
    public partial class ZimmetContext : DbContext
    {
        public ZimmetContext()
        {
        }

        public ZimmetContext(DbContextOptions<ZimmetContext> options)
            : base(options)
        {
        }

        // Dbset'ler artık AssetGuard.Entity'deki Asset, Employee, Category'dir.
        public virtual DbSet<Asset> Assets { get; set; }

        public virtual DbSet<AssetStatus> AssetStatuses { get; set; }

        public virtual DbSet<Assignment> Assignments { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?linkid=723263.
            => optionsBuilder.UseSqlServer("Server=WORK-COMPUTER\\SQLDEV_2022;Database=ZimmetDB;Trusted_Connection=True;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Hatayı veren tabloyu sabitliyoruz:
            modelBuilder.Entity<AssetStatus>().ToTable("AssetStatus");
            modelBuilder.UseIdentityColumns();
            // Güvenlik için diğer tabloları da sabitleyelim
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Department>().ToTable("Departments");
            modelBuilder.Entity<Assignment>().ToTable("Assignments");
            modelBuilder.Entity<Employee>().ToTable("Employees");

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
            // ... (Diğer entity tanımları da buraya gelecek)

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}