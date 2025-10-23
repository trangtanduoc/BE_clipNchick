using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class ProductDto
    {
        public string? Title { get; set; }
        public string? Descript { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
