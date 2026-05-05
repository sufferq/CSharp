namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите число A: ");
            int A = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите число B: ");
            int B = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите число C: ");
            int C = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("============================");

            if (C > A && C > B)
            {
                Console.WriteLine("Места нету даже на один квадрат!");
            }
            else
            {
                int square1 = A / C;
                int square2 = B / C;
                int finalTotal = square1 * square2;
                int free_place = A * B - finalTotal * (C * C);

                Console.WriteLine("Квадратов: " + finalTotal);
                Console.WriteLine("Свободного места: " + free_place);
            }
        }
    }
}
