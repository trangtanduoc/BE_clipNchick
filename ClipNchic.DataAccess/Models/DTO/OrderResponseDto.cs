using System;
using System.Collections.Generic;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class OrderResponseDto
    {
        public int id { get; set; }
        public int? userId { get; set; }
        public UserLiteDto? user { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? name { get; set; }
        public DateTime? createDate { get; set; }
        public decimal? totalPrice { get; set; }
        public decimal? shipPrice { get; set; }
        public decimal? payPrice { get; set; }
        public string? status { get; set; }
        public string? payMethod { get; set; }
        public List<OrderDetailResponseDto> orderDetails { get; set; } = new();
    }

    public class OrderDetailResponseDto
    {
        public int id { get; set; }
        public int? productId { get; set; }
        public ProductLiteDto? product { get; set; }
        public int? blindBoxId { get; set; }
        public BlindBoxLiteDto? blindBox { get; set; }
        public int? quantity { get; set; }
        public decimal? price { get; set; }
    }

    public class ProductLiteDto
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? descript { get; set; }
        public decimal? price { get; set; }
        public string? status { get; set; }
        public ModelLiteDto? model { get; set; }
        public List<ImageLiteDto> images { get; set; } = new();
    }

    public class BlindBoxLiteDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? descript { get; set; }
        public decimal? price { get; set; }
        public List<ImageLiteDto> images { get; set; } = new();
    }

    public class ModelLiteDto
    {
        public string? address { get; set; }
    }

    public class ImageLiteDto
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
    }

    public class UserLiteDto
    {
        public int id { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? image { get; set; }
    }
}
