using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClipNchic.DataAccess.Models;

[Table("User")]
public class User
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? email { get; set; }

    [MaxLength(255)]
    public string? password { get; set; }

    [MaxLength(50)]
    public string? phone { get; set; }

    public DateTime? birthday { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [MaxLength(500)]
    public string? address { get; set; }

    [MaxLength(255)]
    public string? image { get; set; }

    public DateTime? createDate { get; set; }

    [MaxLength(50)]
    public string? status { get; set; }

    public bool isEmailVerified { get; set; } = false;

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<EmailVerificationToken> EmailVerificationTokens { get; set; } = new List<EmailVerificationToken>();
}

[Table("EmailVerificationToken")]
public class EmailVerificationToken
{
    [Key]
    public int id { get; set; }

    public int userId { get; set; }
    public User? User { get; set; }

    [MaxLength(255)]
    public string token { get; set; } = string.Empty;

    public DateTime expiryDate { get; set; }

    public bool isUsed { get; set; } = false;

    public DateTime createdDate { get; set; }
}

[Table("Collection")]
public class Collection
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [MaxLength(500)]
    public string? descript { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<BlindBox> BlindBoxes { get; set; } = new List<BlindBox>();
}

[Table("Model")]
public class Model
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [MaxLength(255)]
    public string? address { get; set; }

    public ICollection<Base> Bases { get; set; } = new List<Base>();
    public ICollection<Charm> Charms { get; set; } = new List<Charm>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

[Table("Image")]
public class Image
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [MaxLength(255)]
    public string? address { get; set; }

    public int? baseId { get; set; }

    public int? charmId { get; set; }

    public int? productId { get; set; }

    public int? blindBoxId { get; set; }
    

}

[Table("Base")]
public class Base
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [MaxLength(100)]
    public string? color { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }

    public Image? Image { get; set; }

    public int? modelId { get; set; }
    public Model? Model { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

[Table("Charm")]
public class Charm
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }

    public int? modelId { get; set; }
    public Model? Model { get; set; }

    public ICollection<CharmProduct> CharmProducts { get; set; } = new List<CharmProduct>();
}

[Table("Product")]
public class Product
{
    [Key]
    public int id { get; set; }

    public int? collectId { get; set; }
    public Collection? Collection { get; set; }

    [MaxLength(255)]
    public string? title { get; set; }

    [MaxLength(500)]
    public string? descript { get; set; }

    public int? baseId { get; set; }
    public Base? Base { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }

    public int? userId { get; set; }
    public User? User { get; set; }

    public int? stock { get; set; }

    public int? modelId { get; set; }
    public Model? Model { get; set; }

    public DateTime? createDate { get; set; }

    [MaxLength(50)]
    public string? status { get; set; }

    public ICollection<CharmProduct> CharmProducts { get; set; } = new List<CharmProduct>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
}

[Table("CharmProduct")]
public class CharmProduct
{
    [Key]
    public int id { get; set; }

    public int? productId { get; set; }
    public Product? Product { get; set; }

    public int? charmId { get; set; }
    public Charm? Charm { get; set; }
}


[Table("BlindBox")]
public class BlindBox
{
    [Key]
    public int id { get; set; }

    public int? collectId { get; set; }
    public Collection? Collection { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [MaxLength(500)]
    public string? descript { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }

    public int? stock { get; set; }

    [MaxLength(50)]
    public string? status { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public ICollection<Image> Images { get; set; } = new List<Image>();
}

[Table("Ship")]
public class Ship
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }
}

[Table("Voucher")]
public class Voucher
{
    [Key]
    public int id { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? discount { get; set; }

    public int? stock { get; set; }

    public DateTime? start { get; set; }

    public DateTime? end { get; set; }
}

[Table("Order")]
public class Order
{
    [Key]
    public int id { get; set; }

    public int? userId { get; set; }
    public User? User { get; set; }

    [MaxLength(50)]
    public string? phone { get; set; }

    [MaxLength(500)]
    public string? address { get; set; }

    [MaxLength(255)]
    public string? name { get; set; }

    public DateTime? createDate { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? totalPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? shipPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? payPrice { get; set; }

    [MaxLength(50)]
    public string? status { get; set; }

    [MaxLength(100)]
    public string? payMethod { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

[Table("OrderDetail")]
public class OrderDetail
{
    [Key]
    public int id { get; set; }

    public int? orderId { get; set; }
    public Order? Order { get; set; }

    public int? productId { get; set; }
    public Product? Product { get; set; }

    public int? blindBoxId { get; set; }
    public BlindBox? BlindBox { get; set; }

    public int? quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? price { get; set; }
}


