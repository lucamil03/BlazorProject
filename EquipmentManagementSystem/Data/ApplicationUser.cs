using Microsoft.AspNetCore.Identity;

namespace EquipmentManagementSystem.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

}