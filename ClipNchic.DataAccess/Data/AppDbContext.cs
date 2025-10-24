using ClipNchic.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ClipNchic.DataAccess.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<Model> Models => Set<Model>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<Base> Bases => Set<Base>();
    public DbSet<Charm> Charms => Set<Charm>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CharmProduct> CharmProducts => Set<CharmProduct>();
    public DbSet<BlindBox> BlindBoxes => Set<BlindBox>();
    public DbSet<Ship> Ships => Set<Ship>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EmailVerificationToken relationships
        modelBuilder.Entity<EmailVerificationToken>()
            .HasOne(evt => evt.User)
            .WithMany(u => u.EmailVerificationTokens)
            .HasForeignKey(evt => evt.userId)
            .OnDelete(DeleteBehavior.Cascade);

        // User relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.userId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.userId)
            .OnDelete(DeleteBehavior.NoAction);

        // Collection relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Collection)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.collectId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<BlindBox>()
            .HasOne(bb => bb.Collection)
            .WithMany(c => c.BlindBoxes)
            .HasForeignKey(bb => bb.collectId)
            .OnDelete(DeleteBehavior.NoAction);

        // Model relationships
        modelBuilder.Entity<Base>()
            .HasOne(b => b.Model)
            .WithMany(m => m.Bases)
            .HasForeignKey(b => b.modelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Charm>()
            .HasOne(c => c.Model)
            .WithMany(m => m.Charms)
            .HasForeignKey(c => c.modelId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Model)
            .WithMany(m => m.Products)
            .HasForeignKey(p => p.modelId)
            .OnDelete(DeleteBehavior.NoAction);     

        // Base relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Base)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.baseId)
            .OnDelete(DeleteBehavior.NoAction);

        // CharmProduct relationships
        modelBuilder.Entity<CharmProduct>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CharmProducts)
            .HasForeignKey(cp => cp.productId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CharmProduct>()
            .HasOne(cp => cp.Charm)
            .WithMany(c => c.CharmProducts)
            .HasForeignKey(cp => cp.charmId)
            .OnDelete(DeleteBehavior.NoAction);   
        // OrderDetail relationships
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.orderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.productId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.BlindBox)
            .WithMany(bb => bb.OrderDetails)
            .HasForeignKey(od => od.blindBoxId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}


