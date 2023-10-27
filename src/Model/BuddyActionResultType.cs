using System.Xml.Serialization;

namespace sodoff.Model
{
    [Flags]
    public enum BuddyActionResultType
    {
        // Token: 0x04000217 RID: 535
        [XmlEnum("0")]
        Unknown = 0,
        // Token: 0x04000218 RID: 536
        [XmlEnum("1")]
        Success = 1,
        // Token: 0x04000219 RID: 537
        [XmlEnum("2")]
        BuddyListFull = 2,
        // Token: 0x0400021A RID: 538
        [XmlEnum("3")]
        FriendBuddyListFull = 3,
        // Token: 0x0400021B RID: 539
        [XmlEnum("4")]
        AlreadyInList = 4,
        // Token: 0x0400021C RID: 540
        [XmlEnum("5")]
        InvalidFriendCode = 5,
        // Token: 0x0400021D RID: 541
        [XmlEnum("6")]
        CannotAddSelf = 6
    }
}
