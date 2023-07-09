using System.ComponentModel.DataAnnotations;

namespace sodoff.Model;

public class MissionState {
    [Key]
    public int Id { get; set; }

    public int MissionId { get; set; }

    public string VikingId { get; set; } = null!;

    public virtual Viking? Viking { get; set; }

    public MissionStatus MissionStatus { get; set; }

    public bool? UserAccepted { get; set; }
}

public enum MissionStatus {
    Upcoming,Active,Completed
}