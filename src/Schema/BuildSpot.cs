using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "BuildSpot", Namespace = "")]
    [Serializable]
    public class BuildSpot
    {
        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "Item")]
        public string Item;
    }
}
