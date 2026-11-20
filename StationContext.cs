using System.Collections.Concurrent;
using BatteryStation.Models;

namespace BatteryStation.Core;

public sealed class StationContext : IDisposable
{
    public SemaphoreSlim ChargerSlots { get; }
    public Mutex CalibrationStand { get; }
    public CancellationTokenSource Cts { get; }
    public CountdownEvent Pending { get; }
    public ConcurrentDictionary<int, CellResult> Results { get; } = new();
    public object LogLock { get; } = new();

    public int Completed, Canceled, Faulted;
    public int SuccessfulCharges, SuccessfulCalibrations;
    public int InterruptedChargerWaits, InterruptedCalibrationWaits;

    public StationContext(int cellCount, int chargerSlots, CancellationTokenSource cts)
    {
        ChargerSlots = new SemaphoreSlim(chargerSlots, chargerSlots);
        CalibrationStand = new Mutex(false);
        Cts = cts;
        Pending = new CountdownEvent(cellCount);
    }

    public void Dispose()
    {
        ChargerSlots.Dispose();
        CalibrationStand.Dispose();
        Cts.Dispose();
        Pending.Dispose();
    }
}