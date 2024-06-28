using System.Xml.Serialization;

namespace sodoff.Schema
{
    [Serializable]
    public class KeyValuePairOfStringString
    {
        [XmlElement(ElementName = "Key")]
        public string? Key { get; set; }
        [XmlElement(ElementName = "Value")]
        public string? Value { get; set; }
    }
}