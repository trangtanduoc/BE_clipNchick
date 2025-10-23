using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class ProductCreateDto
    {
        public int? collectId { get; set; }
        public string? title { get; set; }
        public string? descript { get; set; }
        public int? baseId { get; set; }
        public decimal? price { get; set; } // optional explicit price override
        public int? userId { get; set; }
        public int? stock { get; set; }
        public int? modelId { get; set; }
        public DateTime? createDate { get; set; }
        public string? status { get; set; }
    }
}
