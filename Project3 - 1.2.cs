namespace ConsoleApp1
{

    class User {

        public string surname = string.Empty;
        public string name = string.Empty;
        public string secondname = string.Empty;
        public int age;

        public void Print() {

            Console.WriteLine("ФИО: " + surname + " " + name + " " + secondname);
            Console.WriteLine("Возраст: " + age);
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            User user = new User();

            user.surname = "Иванов";
            user.name = "Иван";
            user.secondname = "Иванович";
            user.age = 21;

            user.Print();
        }
    }
}
