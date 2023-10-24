using sodoff.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Schema
{
    public class HouseData
    {
        [Key, ForeignKey("VikingId")]
        public int Id { get; set; }
        public string VikingId { get; set; } = null!;
        public virtual Viking Viking { get; set; } = null!;
        public string XmlData { get; set; } = null!;
    }
}
