namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите число N(больше нуля): ");
            string N = Console.ReadLine();

            char[] nums = N.ToCharArray();
            Array.Reverse(nums);

            Console.WriteLine(nums);
        }
    }
}
