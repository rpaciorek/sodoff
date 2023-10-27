using System.ComponentModel.DataAnnotations;

namespace sodoff.Schema
{
    public class BuddyRequest
    {
        [Key]
        public string Id { get; set; }

        public string OwnerID { get; set; }

        public string RequestUserID { get; set; }
    }
}
