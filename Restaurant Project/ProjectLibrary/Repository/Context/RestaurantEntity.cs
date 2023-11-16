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
        public virtual DbSet<Password> Passwords { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;

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
                    .HasName("PK__AccountA__33A73E7721E54D75");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
