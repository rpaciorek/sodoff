using System.ComponentModel.DataAnnotations;

namespace sodoff.Model
{
    public class GameDataDb
    {
        [Key]
        public string Id { get; set; } = null!;
        public string VikingId { get; set; } = null!;
        public int RankId { get; set; }
        public int GameId { get; set; }
        public bool IsMultiplayer { get; set; }
        public int GameLevel { get; set; }
        public int Difficulty { get; set; }
        public int Win {  get; set; }
        public int Loss { get; set; }
        public int Score { get; set; }
    }
}
