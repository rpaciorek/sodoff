using sodoff.Schema;
using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "BuddyActionResult", Namespace = "")]
    [Serializable]
    public class BuddyActionResult
    {
        // Token: 0x04000203 RID: 515
        [XmlElement(ElementName = "Result")]
        public BuddyActionResultType Result;

        // Token: 0x04000204 RID: 516
        [XmlElement(ElementName = "Status")]
        public BuddyStatus Status;

        // Token: 0x04000205 RID: 517
        [XmlElement(ElementName = "BuddyUserID")]
        public string BuddyUserID;
    }
}
