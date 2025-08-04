using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StarterApp.Database.Models;

[Table("conference")]
[PrimaryKey(nameof(Id))]
public class Conference
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int SpeakerId { get; set; }

    [ForeignKey(nameof(SpeakerId))]
    public User Speaker { get; set; } = null!;

    public string Location { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime Time { get; set; }
    public int Capacity { get; set; }

    public List<UserConference> UserConferences { get; set; } = new();

    [NotMapped]
    public int NumReservations => UserConferences?.Count ?? 0;
}