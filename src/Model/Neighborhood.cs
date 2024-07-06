using sodoff.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace sodoff.Model
{
    public class Neighborhood
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VikingId { get; set; }

        public Guid Slot0 { get; set; }

        public Guid Slot1 { get; set; }

        public Guid Slot2 { get; set; }

        public Guid Slot3 { get; set; }

        public Guid Slot4 { get; set; }

        public virtual Viking? Viking { get; set; } = null!;
    }
}
