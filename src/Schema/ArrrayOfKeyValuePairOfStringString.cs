using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "ArrayOfKeyValuePairOfStringString")]
    public class ArrayOfKeyValuePairOfStringString
    {
        [XmlElement(ElementName = "KeyValuePairOfStringString")]
        public KeyValuePairOfStringString[]? KeyValuePairOfStringString { get; set; }
    }
}
