using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class ResponseBlindBoxDTO
    {
        public int id { get; set; }

        public int? collectId { get; set; }
        public CollectionDTO? Collection { get; set; }
        public string? name { get; set; }

        public string? descript { get; set; }

        public decimal? price { get; set; }

        public int? stock { get; set; }
        public string? status { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }
}
