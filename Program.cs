using BatteryStation.Core;
using BatteryStation.Models;

namespace BatteryStation;

public static class Program
{
    private const int CellCount = 20;
    private const int ChargerSlots = 4;
    private const int DiagnosticIterations = 100_000;
    private const int ChargeMinMs = 300, ChargeMaxMs = 900;
    private const int CalibMinMs = 100, CalibMaxMs = 300;
    private static readonly int AutoCancelMs = 0;

    private const int ShutdownLimitMs = 10_000;

    public static void Main()
    {
        var rnd = new Random(42);

        ThreadPool.GetAvailableThreads(out int availStart, out int ioStart);
        Console.WriteLine($"Пул потоков: доступно рабочих = {availStart}, IO = {ioStart}");
        Console.WriteLine($"Ячеек: {CellCount}, слотов зарядки: {ChargerSlots}, автоотмена: {(AutoCancelMs > 0 ? AutoCancelMs.ToString() + " мс" : "выкл")}");

        using var cts = new CancellationTokenSource();
        using var ctx = new StationContext(CellCount, ChargerSlots, cts);

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            Journal.Write(ctx, null, "запрошена аварийная остановка (Ctrl+C)");
            SafeCancel(cts);
        };

        Timer? autoTimer = null;
        if (AutoCancelMs > 0)
        {
            autoTimer = new Timer(_ =>
            {
                Journal.Write(ctx, null, $"запрошена аварийная остановка (таймаут {AutoCancelMs} мс)");
                SafeCancel(cts);
            }, null, AutoCancelMs, Timeout.Infinite);
        }

        var cells = new List<Cell>(CellCount);
        for (int i = 1; i <= CellCount; i++)
        {
            var cell = new Cell
            {
                Id = i,
                DiagnosticIterations = DiagnosticIterations,
                ChargeDurationMs = rnd.Next(ChargeMinMs, ChargeMaxMs + 1),
                CalibrationDurationMs = rnd.Next(CalibMinMs, CalibMaxMs + 1)
            };
            cells.Add(cell);
            ctx.Results[i] = new CellResult
            {
                CellId = i,
                Status = CellStatus.Pending,
                StartedAt = DateTime.UtcNow
            };
        }
        Journal.Write(ctx, null, $"партия создана ({cells.Count} ячеек)");

        var totalSw = System.Diagnostics.Stopwatch.StartNew();

        foreach (var cell in cells)
        {
            var state = (ctx, cell);
            ThreadPool.QueueUserWorkItem(CellProcessor.Process, state);
            Journal.Write(ctx, cell.Id, "ячейка поставлена в очередь", CellStatus.Pending);
        }

        bool finished = false;
        try
        {
            finished = ctx.Pending.Wait(ShutdownLimitMs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка ожидания: {ex.Message}");
        }

        if (!finished)
        {
            Console.WriteLine($"ОШИБКА: превышен лимит ожидания завершения ({ShutdownLimitMs} мс)");
            SafeCancel(cts);
        }

        totalSw.Stop();
        autoTimer?.Dispose();

        ThreadPool.GetAvailableThreads(out int availEnd, out int ioEnd);

        Console.WriteLine();
        Console.WriteLine("===== ИТОГИ =====");
        Console.WriteLine($"Всего ячеек:               {CellCount}");
        Console.WriteLine($"Completed:                 {ctx.Completed}");
        Console.WriteLine($"Canceled:                  {ctx.Canceled}");
        Console.WriteLine($"Faulted:                   {ctx.Faulted}");
        Console.WriteLine($"Сумма статусов:            {ctx.Completed + ctx.Canceled + ctx.Faulted}");
        Console.WriteLine($"Время выполнения, мс:      {totalSw.ElapsedMilliseconds}");
        Console.WriteLine($"Потоков пула (нач/кон):    {availStart} / {availEnd}");
        Console.WriteLine($"Успешных зарядок:          {ctx.SuccessfulCharges}");
        Console.WriteLine($"Успешных калибровок:       {ctx.SuccessfulCalibrations}");
        Console.WriteLine($"Прервано ожиданий зарядки: {ctx.InterruptedChargerWaits}");
        Console.WriteLine($"Прервано ожиданий калибр.: {ctx.InterruptedCalibrationWaits}");
        Journal.Write(ctx, null, "обработка партии завершена");

        if (ctx.Completed + ctx.Canceled + ctx.Faulted != CellCount)
            Console.WriteLine("ВНИМАНИЕ: сумма статусов не совпадает с числом ячеек!");
    }

    private static void SafeCancel(CancellationTokenSource cts)
    {
        try { cts.Cancel(); }
        catch (ObjectDisposedException) { }
    }
}