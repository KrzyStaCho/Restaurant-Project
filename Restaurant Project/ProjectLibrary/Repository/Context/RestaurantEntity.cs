using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectLibrary.Repository.Entity;

namespace ProjectLibrary.Repository.Context;

public partial class RestaurantEntity : DbContext
{
    public RestaurantEntity()
    {
    }

    public RestaurantEntity(DbContextOptions<RestaurantEntity> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountArchive> AccountArchives { get; set; }

    public virtual DbSet<AccountGroup> AccountGroups { get; set; }

    public virtual DbSet<MeasureUnit> MeasureUnits { get; set; }

    public virtual DbSet<Password> Passwords { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductArchive> ProductArchives { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }

    public virtual DbSet<RecipeDetail> RecipeDetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierArchive> SupplierArchives { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KRZYSZTOF_PC\\SQLEXPRESS;TrustServerCertificate=True;Integrated Security=True;Database=Restaurant");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA586DF29ED7A");

            entity.Property(e => e.CanChanged).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Group).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__GroupID__2F10007B");
        });

        modelBuilder.Entity<AccountArchive>(entity =>
        {
            entity.HasKey(e => e.ArchiveId).HasName("PK__AccountA__33A73E77D21C5270");
        });

        modelBuilder.Entity<AccountGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__AccountG__149AF30AA00F441F");

            entity.Property(e => e.CanChanged).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Groups)
                .UsingEntity<Dictionary<string, object>>(
                    "AccountGroupPerm",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AccountGr__Permi__3A81B327"),
                    l => l.HasOne<AccountGroup>().WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__AccountGr__Group__398D8EEE"),
                    j =>
                    {
                        j.HasKey("GroupId", "PermissionId").HasName("PK__AccountG__FA609CBA1BDFAE52");
                        j.ToTable("AccountGroupPerm");
                    });
        });

        modelBuilder.Entity<MeasureUnit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__MeasureU__44F5EC95DCCAABD9");
        });

        modelBuilder.Entity<Password>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Password__349DA5864F81266F");

            entity.ToTable("Password", tb => tb.HasTrigger("RemoveAccountTrigger"));

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Account).WithOne(p => p.Password)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Password__Accoun__32E0915F");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB0F9D2B5F87");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6ED0293977D");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__Categor__6D0D32F4");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__Supplie__6E01572D");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__UnitID__6C190EBB");
        });

        modelBuilder.Entity<ProductArchive>(entity =>
        {
            entity.HasKey(e => e.ArchiveId).HasName("PK__ProductA__33A73E77E829FB26");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__19093A2B68C4C647");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("PK__Recipe__FDD988D0861856CA");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Recipe__Category__05D8E0BE");
        });

        modelBuilder.Entity<RecipeCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__RecipeCa__19093A2B7FF1E6D1");
        });

        modelBuilder.Entity<RecipeDetail>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.ProductId }).HasName("PK__RecipeDe__369944BE0AA7D357");

            entity.HasOne(d => d.Product).WithMany(p => p.RecipeDetails).HasConstraintName("FK__RecipeDet__Produ__0A9D95DB");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeDetails).HasConstraintName("FK__RecipeDet__Recip__09A971A2");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666940B84BA87");

            entity.Property(e => e.LastModified).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<SupplierArchive>(entity =>
        {
            entity.HasKey(e => e.ArchiveId).HasName("PK__Supplier__33A73E775497490C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
