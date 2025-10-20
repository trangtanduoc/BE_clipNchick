using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class OrderDTO
    {
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? PayMethod { get; set; }

        public decimal? TotalPrice { get; set; }
        public decimal? ShipPrice { get; set; }
        public decimal? PayPrice { get; set; }
    }

}
