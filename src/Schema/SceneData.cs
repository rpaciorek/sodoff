using sodoff.Model;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace sodoff.Schema
{
    public class SceneData
    {
        [Key]
        public int Id { get; set; }
        public string VikingId { get; set; } = null!;
        public string SceneName { get; set; } = null!;
        public string XmlData {  get; set; } = null!;
        public Viking Viking { get; set; } = null!;
    }
}
