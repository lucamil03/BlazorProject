using System.ComponentModel.DataAnnotations;

namespace EquipmentManagementSystem.Data;

public class Tool
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
