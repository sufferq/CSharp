namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите число A: ");
            int A = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите число B: ");
            int B = Convert.ToInt32(Console.ReadLine());

            if (A > B)
            {
                Console.WriteLine("Число A не может быть больше числа B!");
            }
            else
            {
                for (int i = A; i <= B; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        Console.Write(i);
                    }
                    Console.WriteLine(" ");
                }
            }
        }
    }
}
