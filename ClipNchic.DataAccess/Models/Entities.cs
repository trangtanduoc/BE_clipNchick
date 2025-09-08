using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClipNchic.DataAccess.Models;

[Table("Roles")]
public class Role
{
    [Key]
    public int RoleId { get; set; }

    [Required, MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}

[Table("Users")]
public class User
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(100)]
    public string? FullName { get; set; }

    [Required, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    public int? RoleId { get; set; }
    public Role? Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<CustomDesign> CustomDesigns { get; set; } = new List<CustomDesign>();
}

[Table("ProductModels")]
public class ProductModel
{
    [Key]
    public int Model3DId { get; set; }

    [MaxLength(255)]
    public string? FileName { get; set; }

    [MaxLength(500)]
    public string? FilePath { get; set; }

    [MaxLength(50)]
    public string? FileType { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<CustomDesign> CustomDesigns { get; set; } = new List<CustomDesign>();
}

[Table("Products")]
public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required, MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [MaxLength(500)]
    public string? PreviewImage { get; set; }

    public int? Model3DId { get; set; }
    public ProductModel? Model3D { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<CustomDesign> CustomDesigns { get; set; } = new List<CustomDesign>();
}

[Table("Materials")]
public class Material
{
    [Key]
    public int MaterialId { get; set; }

    [MaxLength(100)]
    public string? MaterialName { get; set; }

    public string? Description { get; set; }

    [MaxLength(20)]
    public string? ColorCode { get; set; }

    public ICollection<CustomDesign> CustomDesigns { get; set; } = new List<CustomDesign>();
}

[Table("Textures")]
public class Texture
{
    [Key]
    public int TextureId { get; set; }

    [MaxLength(100)]
    public string? TextureName { get; set; }

    public string? Description { get; set; }

    [MaxLength(500)]
    public string? TexturePath { get; set; }

    public ICollection<CustomDesign> CustomDesigns { get; set; } = new List<CustomDesign>();
}

[Table("Accessories")]
public class Accessory
{
    [Key]
    public int AccessoryId { get; set; }

    [MaxLength(100)]
    public string? AccessoryName { get; set; }

    public string? Description { get; set; }

    [MaxLength(500)]
    public string? AccessoryImage { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public ICollection<CustomDesignAccessory> CustomDesignAccessories { get; set; } = new List<CustomDesignAccessory>();
}

[Table("CustomDesigns")]
public class CustomDesign
{
    [Key]
    public int DesignId { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public int? BaseProductId { get; set; }
    public Product? BaseProduct { get; set; }

    public int? MaterialId { get; set; }
    public Material? Material { get; set; }

    public int? TextureId { get; set; }
    public Texture? Texture { get; set; }

    public int? Model3DId { get; set; }
    public ProductModel? Model3D { get; set; }

    [MaxLength(500)]
    public string? PreviewImage { get; set; }

    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<CustomDesignAccessory> CustomDesignAccessories { get; set; } = new List<CustomDesignAccessory>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

[Table("CustomDesignAccessories")]
public class CustomDesignAccessory
{
    public int DesignId { get; set; }
    public CustomDesign? Design { get; set; }

    public int AccessoryId { get; set; }
    public Accessory? Accessory { get; set; }

    public int Quantity { get; set; } = 1;
}

[Table("Orders")]
public class Order
{
    [Key]
    public int OrderId { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }

    public DateTime OrderDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

[Table("OrderDetails")]
public class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }
    public Order? Order { get; set; }

    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    public int? DesignId { get; set; }
    public CustomDesign? Design { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
}


