namespace EquipmentManagementSystem.Data;

public static class ToolStatusHelper
{
    public static ToolStatus GetCurrentStatus(Tool tool, List<Reservation> reservations)
    {
        var now = DateOnly.FromDateTime(DateTime.Now);

        var activeReservation = reservations
            .FirstOrDefault(r => r.ToolId == tool.Id && r.StartDate <= now && r.EndDate >= now);

        if (activeReservation != null)
        {
            return activeReservation.Type == ReservationType.Wartung
                ? ToolStatus.Wartung
                : ToolStatus.Ausgeliehen;
        }

        return ToolStatus.Verfügbar;
    }

    public static string GetStatusLabel(Tool tool, List<Reservation> reservations)
    {
        return GetCurrentStatus(tool, reservations) switch
        {
            ToolStatus.Verfügbar => "✅ Verfügbar",
            ToolStatus.Ausgeliehen => "🔴 Ausgeliehen",
            ToolStatus.Wartung => "🛠️ Wartung",
            _ => "Unbekannt"
        };
    }
}

public enum ToolStatus
{
    Verfügbar,
    Ausgeliehen,
    Wartung
}