using System.ComponentModel.DataAnnotations;
using System.Data;

namespace sodoff.Model
{
    public class BuddyRelation
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public string OwnerID { get; set; } = null!;

        public string BuddyID { get; set; } = null!;

    }
}
