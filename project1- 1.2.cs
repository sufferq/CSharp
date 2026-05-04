namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double rubs = 10000;
            int K = 1;
            double SumOfProcent = 0;
            double P2 = 0;

            Console.Write("Введите процент: ");
            double P = Convert.ToDouble(Console.ReadLine());

            P2 = P / 100;

            do {
                Console.WriteLine("Месяц " + K + ":");

                SumOfProcent = rubs * P2;
                rubs = SumOfProcent + rubs;
                K++;

                Console.WriteLine("Банк добавил " + P + "% от 10000 = " + Math.Round(SumOfProcent, 1));
                Console.WriteLine("Сумма стала = " + Math.Round(rubs, 1));
                Console.WriteLine("========================================");

                if (rubs >= 11000.0)
                {
                    Console.WriteLine(Math.Round(rubs, 1) + " больше 11000! Останаваливаемся");
                    break;
                }
                else {
                    continue;
                }
            
            }while(true);

            Console.WriteLine("========================================");
            Console.WriteLine("Итоговая сумма: " + Math.Round(rubs, 1));
            Console.WriteLine("Через " + K + " месяца");
        }
    }
}
