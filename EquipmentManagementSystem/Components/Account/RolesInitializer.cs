using Microsoft.AspNetCore.Identity;

namespace EquipmentManagementSystem.Components.Account;

public static class RolesInitializer
{
    public static async Task InitializeRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Admin", "User", "Pending" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
