namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = new int[] { 6, 0, -2, 4, -4, -6, 1 };

            Array.Sort(arr);

            for (int i = 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i] + " ");
            }  
        }
    }
}
