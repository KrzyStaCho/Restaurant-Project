using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectLibrary.Repository.Entity;

namespace ProjectLibrary.Repository.Context
{
    public partial class RestaurantEntity : DbContext
    {
        public RestaurantEntity()
        {
        }

        public RestaurantEntity(DbContextOptions<RestaurantEntity> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountArchive> AccountArchives { get; set; } = null!;
        public virtual DbSet<AccountGroup> AccountGroups { get; set; } = null!;
        public virtual DbSet<MeasureUnit> MeasureUnits { get; set; } = null!;
        public virtual DbSet<Password> Passwords { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductArchive> ProductArchives { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<SupplierArchive> SupplierArchives { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=KRZYSZTOF_PC\\SQLEXPRESS;TrustServerCertificate=True;Integrated Security=True;Database=Restaurant");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.CanChanged).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__GroupID__2F10007B");
            });

            modelBuilder.Entity<AccountArchive>(entity =>
            {
                entity.HasKey(e => e.ArchiveId)
                    .HasName("PK__AccountA__33A73E77D21C5270");
            });

            modelBuilder.Entity<AccountGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PK__AccountG__149AF30AA00F441F");

                entity.Property(e => e.CanChanged).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

                entity.HasMany(d => d.Permissions)
                    .WithMany(p => p.Groups)
                    .UsingEntity<Dictionary<string, object>>(
                        "AccountGroupPerm",
                        l => l.HasOne<Permission>().WithMany().HasForeignKey("PermissionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__AccountGr__Permi__3A81B327"),
                        r => r.HasOne<AccountGroup>().WithMany().HasForeignKey("GroupId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__AccountGr__Group__398D8EEE"),
                        j =>
                        {
                            j.HasKey("GroupId", "PermissionId").HasName("PK__AccountG__FA609CBA1BDFAE52");

                            j.ToTable("AccountGroupPerm");

                            j.IndexerProperty<int>("GroupId").HasColumnName("GroupID");

                            j.IndexerProperty<int>("PermissionId").HasColumnName("PermissionID");
                        });
            });

            modelBuilder.Entity<MeasureUnit>(entity =>
            {
                entity.HasKey(e => e.UnitId)
                    .HasName("PK__MeasureU__44F5EC95DCCAABD9");
            });

            modelBuilder.Entity<Password>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__Password__349DA5864F81266F");

                entity.Property(e => e.AccountId).ValueGeneratedNever();

                entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Password)
                    .HasForeignKey<Password>(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Password__Accoun__32E0915F");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Product__Categor__6D0D32F4");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__Product__Supplie__6E01572D");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Product__UnitID__6C190EBB");
            });

            modelBuilder.Entity<ProductArchive>(entity =>
            {
                entity.HasKey(e => e.ArchiveId)
                    .HasName("PK__ProductA__33A73E77E829FB26");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .HasName("PK__ProductC__19093A2B68C4C647");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<SupplierArchive>(entity =>
            {
                entity.HasKey(e => e.ArchiveId)
                    .HasName("PK__Supplier__33A73E775497490C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
