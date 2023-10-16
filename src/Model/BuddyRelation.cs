using System.ComponentModel.DataAnnotations;
using System.Data;

namespace sodoff.Model
{
    public class BuddyRelation
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public Guid OwnerID { get; set; }

        public Guid BuddyID { get; set; }

    }
}
