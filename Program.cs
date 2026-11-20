using System;
using System.Diagnostics;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace TwoThreadsApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.UTF8;

            Thread thread1 = new Thread(NotepadWorker) { Name = "Поток 1" };
            Thread thread2 = new Thread(CmdWorker) { Name = "Поток 2" };

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.WriteLine("[Main] Все потоки завершены. Нажмите Enter для выхода.");
            Console.ReadLine();
        }

        static void NotepadWorker()
        {
            Console.WriteLine("[Поток 1] Запуск потока.");
            Console.WriteLine("[Поток 1] Запуск Notepad.");

            Process.Start("notepad.exe");

            Console.WriteLine("[Поток 1] Ожидание завершения Notepad...");
            Thread.Sleep(3000); 

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "taskkill.exe",
                    Arguments = "/F /IM notepad.exe /T", 
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (Process killProcess = Process.Start(psi))
                {
                    killProcess.WaitForExit(); 

                    if (killProcess.ExitCode == 0)
                    {
                        Console.WriteLine("[Поток 1] Notepad закрыт.");
                    }
                    else
                    {
                        Console.WriteLine("[Поток 1] Не удалось закрыть Notepad (возможно, он уже закрыт).");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Поток 1] Ошибка при закрытии Notepad: {ex.Message}");
            }
        }

        static void CmdWorker()
        {
            Console.WriteLine("[Поток 2] Запуск потока.");

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.CreateNoWindow = true;

            cmd.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            cmd.StartInfo.StandardErrorEncoding = Encoding.GetEncoding(866);

            Console.WriteLine("[Поток 2] Запуск cmd.exe.");
            cmd.Start();

            cmd.StandardInput.WriteLine("@echo off");

            Console.WriteLine("[Поток 2] Отправка команды 'dir'.");
            cmd.StandardInput.WriteLine("dir");

            cmd.StandardInput.WriteLine("exit");
            cmd.StandardInput.Flush();

            string output = cmd.StandardOutput.ReadToEnd();
            string error = cmd.StandardError.ReadToEnd();

            cmd.WaitForExit();

            Console.WriteLine("[Поток 2] Результат команды dir:");
            Console.WriteLine("--------------------------------------------------");

            string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();

                if (trimmedLine.StartsWith("Volume in drive") ||
                    trimmedLine.StartsWith("Volume Serial Number") ||
                    trimmedLine.StartsWith("Directory of") ||
                    trimmedLine.Contains("File(s)") ||
                    trimmedLine.Contains("Dir(s)") ||
                    trimmedLine.Contains("bytes free") ||
                    trimmedLine.Contains("байт свободно") ||
                    trimmedLine.Contains("байт занято") ||
                    trimmedLine.StartsWith("Том в устройстве") ||
                    trimmedLine.StartsWith("Серийный номер тома"))
                {
                    continue; 
                }

                if (!string.IsNullOrWhiteSpace(trimmedLine))
                {
                    Console.WriteLine(trimmedLine);
                }
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("[Поток 2] Ошибки:");
                Console.WriteLine(error);
            }

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("[Поток 2] Завершение потока.");
        }
    }
}