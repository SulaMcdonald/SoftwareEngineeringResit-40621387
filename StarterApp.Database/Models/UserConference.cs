using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StarterApp.Database.Models;

[Table("user_conference")]
[PrimaryKey(nameof(UserId), nameof(ConferenceId))]
public class UserConference
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ConferenceId { get; set; }
    public Conference Conference { get; set; } = null!;
    
    public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
}
