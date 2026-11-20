namespace BatteryStation.Models;

public sealed class Cell
{
    public int Id { get; init; }
    public int DiagnosticIterations { get; init; }
    public int ChargeDurationMs { get; init; }
    public int CalibrationDurationMs { get; init; }
}