using System.Xml.Serialization;

[XmlRoot(ElementName = "BuddyLocation", Namespace = "")]
[Serializable]
public class BuddyLocation
{
    [XmlElement(ElementName = "UserID")]
    public string UserID;

    [XmlElement(ElementName = "Server")]
    public string Server;

    [XmlElement(ElementName = "Zone")]
    public string Zone;

    [XmlElement(ElementName = "Room")]
    public string Room;

    [XmlElement(ElementName = "MultiplayerID")]
    public int MultiplayerID;

    [XmlElement(ElementName = "ServerVersion")]
    public string ServerVersion;

    [XmlElement(ElementName = "AppName")]
    public string AppName;

    [XmlElement(ElementName = "ServerPort")]
    public int ServerPort;
}