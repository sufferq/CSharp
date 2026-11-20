namespace BatteryStation.Models;

public sealed class CellResult
{
    public int CellId { get; init; }
    public CellStatus Status { get; set; }
    public DateTime StartedAt { get; init; }
    public DateTime? FinishedAt { get; set; }
    public string? ErrorMessage { get; set; }
}