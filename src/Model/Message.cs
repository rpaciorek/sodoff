using sodoff.Schema;
using System.ComponentModel.DataAnnotations;

namespace sodoff.Model
{
    public class Message
    {
        public virtual Viking? Viking { get; set; }

        [Key]
        public int Id { get; set; }
        public int VikingId { get; set; }
        public int? FromVikingId { get; set; }

        public int QueueID { get; set; }
        public int? ConversationID { get; set; }
        public int? ReplyToMessageID { get; set; }
        public MessageType? MessageType { get; set; }
        public MessageTypeID? MessageTypeID { get; set; }
        public MessageLevel MessageLevel { get; set; }
        public string? Data { get; set; }
        public string? MemberMessage { get; set; }
        public string? NonMemberMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
    }
}
