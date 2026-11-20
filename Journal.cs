using BatteryStation.Models;

namespace BatteryStation.Core;

public static class Journal
{
    public static void Write(StationContext ctx, int? cellId, string message, CellStatus? status = null)
    {
        lock (ctx.LogLock)
        {
            var ts = DateTime.Now.ToString("HH:mm:ss.fff");
            var tid = Environment.CurrentManagedThreadId;
            var cellPart = cellId.HasValue ? $"Cell#{cellId.Value:D2}" : "------";
            var statusPart = status.HasValue ? $" [{status.Value}]" : "";
            Console.WriteLine($"[{ts}] [T{tid:D2}] [{cellPart}] {message}{statusPart}");
        }
    }
}