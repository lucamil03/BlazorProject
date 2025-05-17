using System.ComponentModel.DataAnnotations;

namespace EquipmentManagementSystem.Data;

public enum ToolStatus
{
    Verfügbar = 0,
    Ausgeliehen = 1,
    Wartung = 2
}

public class Tool
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public ToolStatus Status { get; set; } = ToolStatus.Verfügbar;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
