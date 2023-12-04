using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "Module", Namespace = "")]
    [Serializable]
    public class Module
    {
        [XmlElement(ElementName = "Name")]
        public string Name;

        [XmlElement(ElementName = "Stars")]
        public int Stars;
    }
}
