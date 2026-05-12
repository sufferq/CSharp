namespace ConsoleApp7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Массив: {");

            int[] arr = new int[20];
            Random random = new Random();

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(0, 11);
            }

            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }

            Console.WriteLine("}");
            Console.WriteLine("==========================");


            Console.Write("Введите число для посчёта: ");
            int num = Convert.ToInt32 (Console.ReadLine());

            int count = 0;

            for (int i = 0; i < arr.Length; i++)
            {
                if (num == arr[i])
                {
                    count++;
                }
            }

            Console.WriteLine("Число " + num + " повторилось " + count + " раз.");
        }
    }
}
