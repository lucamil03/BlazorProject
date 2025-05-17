using System.ComponentModel.DataAnnotations;

namespace EquipmentManagementSystem.Data;

public enum ReservationType
{
    Reservierung,
    Wartung
}
public class Reservation
{
    public int Id { get; set; }

    [Required]
    public int ToolId { get; set; }

    public Tool Tool { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
    
    public ReservationType Type { get; set; } = ReservationType.Reservierung;

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }
}
