using System.Xml.Serialization;

namespace sodoff.Schema
{
    [XmlRoot("data")]
    public class XmlGameData
    {
        [XmlElement("highscore")]
        public int HighScore;
    }
}
