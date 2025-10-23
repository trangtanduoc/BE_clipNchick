using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipNchic.DataAccess.Models.DTO
{
    public class ResponseProductDTO
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? descript { get; set; }
        public int? stock { get; set; }
        public decimal? Totalprice { get; set; }
        public DateTime? createDate { get; set; }
        public string? status { get; set; }
        public int? collectId { get; set; }
        public CollectionDTO? Collection { get; set; }
        public int? baseId { get; set; }
        public Base? Base { get; set; }
        public int? modelId { get; set; }
        public Model? Model { get; set; }
        public List<CharmProduct>? CharmProducts { get; set; } = new();
        public List<Image>? Images { get; set; } = new();
    }
}
