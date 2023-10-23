using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot(ElementName = "SceneData", Namespace = "", IsNullable = true)]
    [Serializable]
    public class SceneData
    {
        [XmlArrayItem("BuildSpot", IsNullable = false)]
        public BuildSpot[] BuildSpots;

        [XmlArrayItem("Module", IsNullable = false)]
        public Module[] Modules;
    }
}
