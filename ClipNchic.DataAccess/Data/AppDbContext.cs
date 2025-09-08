using ClipNchic.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ProductModel> ProductModels => Set<ProductModel>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Texture> Textures => Set<Texture>();
    public DbSet<Accessory> Accessories => Set<Accessory>();
    public DbSet<CustomDesign> CustomDesigns => Set<CustomDesign>();
    public DbSet<CustomDesignAccessory> CustomDesignAccessories => Set<CustomDesignAccessory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Model3D)
            .WithMany(m => m.Products)
            .HasForeignKey(p => p.Model3DId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomDesign>()
            .HasOne(cd => cd.User)
            .WithMany(u => u.CustomDesigns)
            .HasForeignKey(cd => cd.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomDesign>()
            .HasOne(cd => cd.BaseProduct)
            .WithMany(p => p.CustomDesigns)
            .HasForeignKey(cd => cd.BaseProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomDesign>()
            .HasOne(cd => cd.Material)
            .WithMany(m => m.CustomDesigns)
            .HasForeignKey(cd => cd.MaterialId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomDesign>()
            .HasOne(cd => cd.Texture)
            .WithMany(t => t.CustomDesigns)
            .HasForeignKey(cd => cd.TextureId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomDesign>()
            .HasOne(cd => cd.Model3D)
            .WithMany(m => m.CustomDesigns)
            .HasForeignKey(cd => cd.Model3DId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CustomDesignAccessory>()
            .HasKey(cda => new { cda.DesignId, cda.AccessoryId });

        modelBuilder.Entity<CustomDesignAccessory>()
            .HasOne(cda => cda.Design)
            .WithMany(cd => cd.CustomDesignAccessories)
            .HasForeignKey(cda => cda.DesignId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CustomDesignAccessory>()
            .HasOne(cda => cda.Accessory)
            .WithMany(a => a.CustomDesignAccessories)
            .HasForeignKey(cda => cda.AccessoryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Design)
            .WithMany(cd => cd.OrderDetails)
            .HasForeignKey(od => od.DesignId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


