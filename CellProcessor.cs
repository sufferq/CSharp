using BatteryStation.Models;

namespace BatteryStation.Core;

public static class CellProcessor
{
    public static void Process(object? state)
    {
        var (ctx, cell) = ((StationContext, Cell))state!;
        var token = ctx.Cts.Token;
        var result = ctx.Results[cell.Id];

        try
        {
            if (token.IsCancellationRequested)
            {
                MarkCanceled(ctx, result);
                return;
            }

            // ДИАГНОСТИКА
            result.Status = CellStatus.Diagnostics;
            Journal.Write(ctx, cell.Id, "диагностика начата", result.Status);

            long acc = 1;
            for (int i = 0; i < cell.DiagnosticIterations; i++)
            {
                acc = (acc * 31 + i) % 1_000_000_007;
                if ((i & 0x3FF) == 0) token.ThrowIfCancellationRequested();
            }
            token.ThrowIfCancellationRequested();
            Journal.Write(ctx, cell.Id, "диагностика завершена", result.Status);

            // ЗАРЯДКА
            result.Status = CellStatus.WaitingForCharger;
            Journal.Write(ctx, cell.Id, "ожидание слота зарядки", result.Status);

            bool chargerAcquired = false;
            try
            {
                chargerAcquired = ctx.ChargerSlots.Wait(Timeout.Infinite, token);
                if (!chargerAcquired)
                {
                    Interlocked.Increment(ref ctx.InterruptedChargerWaits);
                    MarkCanceled(ctx, result);
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                Interlocked.Increment(ref ctx.InterruptedChargerWaits);
                MarkCanceled(ctx, result);
                return;
            }

            try
            {
                result.Status = CellStatus.Charging;
                Journal.Write(ctx, cell.Id, "слот зарядки получен", result.Status);
                Journal.Write(ctx, cell.Id, "зарядка начата", result.Status);

                SimulateDelay(cell.ChargeDurationMs, token);
                token.ThrowIfCancellationRequested();

                Journal.Write(ctx, cell.Id, "зарядка завершена", result.Status);
                Interlocked.Increment(ref ctx.SuccessfulCharges);
            }
            catch (OperationCanceledException)
            {
                Journal.Write(ctx, cell.Id, "зарядка прервана", result.Status);
                MarkCanceled(ctx, result);
                return;
            }
            finally
            {
                if (chargerAcquired)
                {
                    ctx.ChargerSlots.Release();
                    Journal.Write(ctx, cell.Id, "слот зарядки освобождён", result.Status);
                }
            }

            // КАЛИБРОВКА
            result.Status = CellStatus.WaitingForCalibration;
            Journal.Write(ctx, cell.Id, "ожидание калибровочного стенда", result.Status);

            bool mutexAcquired = false;
            try
            {
                int idx = WaitHandle.WaitAny(new WaitHandle[]
                {
                    ctx.CalibrationStand,
                    token.WaitHandle
                });

                if (idx == 1)
                {
                    Interlocked.Increment(ref ctx.InterruptedCalibrationWaits);
                    MarkCanceled(ctx, result);
                    return;
                }

                mutexAcquired = true;

                if (token.IsCancellationRequested)
                {
                    Interlocked.Increment(ref ctx.InterruptedCalibrationWaits);
                    MarkCanceled(ctx, result);
                    return;
                }

                result.Status = CellStatus.Calibration;
                Journal.Write(ctx, cell.Id, "калибровочный стенд получен", result.Status);
                Journal.Write(ctx, cell.Id, "калибровка начата", result.Status);

                SimulateDelay(cell.CalibrationDurationMs, token);
                token.ThrowIfCancellationRequested();

                Journal.Write(ctx, cell.Id, "калибровка завершена", result.Status);
                Interlocked.Increment(ref ctx.SuccessfulCalibrations);
            }
            catch (OperationCanceledException)
            {
                Journal.Write(ctx, cell.Id, "калибровка прервана", result.Status);
                MarkCanceled(ctx, result);
                return;
            }
            finally
            {
                if (mutexAcquired)
                {
                    ctx.CalibrationStand.ReleaseMutex();
                    Journal.Write(ctx, cell.Id, "калибровочный стенд освобождён", result.Status);
                }
            }

            // ЗАВЕРШЕНИЕ
            result.Status = CellStatus.Completed;
            result.FinishedAt = DateTime.UtcNow;
            Interlocked.Increment(ref ctx.Completed);
            Journal.Write(ctx, cell.Id, "ячейка завершена", result.Status);
        }
        catch (OperationCanceledException)
        {
            MarkCanceled(ctx, result);
        }
        catch (Exception ex)
        {
            result.Status = CellStatus.Faulted;
            result.FinishedAt = DateTime.UtcNow;
            result.ErrorMessage = ex.Message;
            Interlocked.Increment(ref ctx.Faulted);
            Journal.Write(ctx, cell.Id, $"произошла ошибка: {ex.Message}", result.Status);
        }
        finally
        {
            ctx.Pending.Signal();
        }
    }

    private static void MarkCanceled(StationContext ctx, CellResult result)
    {
        if (result.Status == CellStatus.Canceled) return;
        result.Status = CellStatus.Canceled;
        result.FinishedAt ??= DateTime.UtcNow;
        Interlocked.Increment(ref ctx.Canceled);
        Journal.Write(ctx, result.CellId, "ячейка отменена", result.Status);
    }

    private static void SimulateDelay(int ms, CancellationToken token)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < ms)
        {
            token.ThrowIfCancellationRequested();
            Thread.SpinWait(50_000);
            if (token.WaitHandle.WaitOne(10))
                token.ThrowIfCancellationRequested();
        }
    }
}