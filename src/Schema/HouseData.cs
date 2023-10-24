using sodoff.Model;
using System.ComponentModel.DataAnnotations;

namespace sodoff.Schema
{
    public class HouseData
    {
        [Key]
        public int Id { get; set; }
        public virtual Viking Viking { get; set; } = null!;
        public string XmlData { get; set; } = null!;
    }
}
