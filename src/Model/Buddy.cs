using sodoff.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace sodoff.Model
{
    public class Buddy
    {
        public virtual Viking? Viking { get; set; }

        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public int OwnerID { get; set; }

        public int BuddyID { get; set; }

        public bool BestBuddy { get; set; }

        public BuddyStatus Status { get; set; }
    }
}
