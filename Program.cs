namespace ConsoleApp5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[]{ 1, 2, 3, 0, 0, 4, 5, 0, 7, 0 };
            int notZero = 0;

            Console.WriteLine("Изначальный массив: ");

            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();

            Console.WriteLine("============================");


            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0)
                {
                    arr[notZero] = arr[i];
                    notZero++;
                } 
            }

            for (int i = notZero; i < arr.Length; i++)
            {
                arr[i] = -1;
            }

            Console.WriteLine("Сжатый массив: ");

            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
