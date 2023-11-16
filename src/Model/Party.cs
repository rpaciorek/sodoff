using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sodoff.Model
{
    public class Party
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Location { get; set; } = null!;
        public string VikingId { get; set; } = null!;
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
        public bool? PrivateParty { get; set; }
        public string LocationIconAsset { get; set; } = null!;
    }
}
