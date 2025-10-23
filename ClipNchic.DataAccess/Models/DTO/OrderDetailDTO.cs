using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class OrderDetailDTO
    {
        public int? ProductId { get; set; }
        public int? BlindBoxId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
