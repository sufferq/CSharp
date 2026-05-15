namespace ConsoleApp1
{

    class FeaturesBooks
    {

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string autor = string.Empty;
        public string Autor
        {
            get { return autor; }
        }

        public string PublishingHouse { get; private set; } = string.Empty;

        public int yearOfRelease;
        public int pageCount;


        public FeaturesBooks(string name, string autor, string publishingHouse, int yearOfRelease, int pageCount)
        {
            this.name = name;
            this.autor = autor;
            PublishingHouse = publishingHouse;
            this.yearOfRelease = yearOfRelease;
            this.pageCount = pageCount;
        }


        public string Info()
        {

            return $"Название: {name}, " +
                   $"Автор: {autor}, " +
                   $"Год выпуска: {yearOfRelease}, " +
                   $"Объем: {pageCount} листов, " +
                   $"Издательство: {PublishingHouse}";

        }
    }

    class MethodBooks
    {
        public string name = string.Empty;
        public string autor = string.Empty;
        public string publishingHouse = string.Empty;
        public int yearOfRelease;
        public int pageCount;


        public MethodBooks(string name, string autor, string publishingHouse, int yearOfRelease, int pageCount)
        {
            this.name = name;
            this.autor = autor;
            this.publishingHouse = publishingHouse;
            this.yearOfRelease = yearOfRelease;
            this.pageCount = pageCount;
        }
        public string Info()
        {
            return $"Название: {name}, " +
                   $"Автор: {autor}, " +
                   $"Год выпуска: {yearOfRelease}, " +
                   $"Объем: {pageCount} листов, " +
                   $"Издательство: {publishingHouse}";
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            FeaturesBooks featuresBooks = new FeaturesBooks("Война и мир", "Лев Толстой", "АСТ", 1869, 1300);
            MethodBooks methodBooks = new MethodBooks("Война и мир", "Лев Толстой", "АСТ", 1869, 1300);
            Console.Write("Со свойставами: ");
            Console.WriteLine(featuresBooks.Info());
            Console.WriteLine("\n");
            Console.Write("Без свойств: ");
            Console.WriteLine(methodBooks.Info());
            Console.WriteLine("\n");

            featuresBooks.Name = "Анна Каренина";
            //featuresBooks.Autor = "New autor"; Будет ошибка! Так как он только для чтения
            Console.Write("После изменения первого класса со свойствами: ");
            Console.WriteLine(featuresBooks.Info());
            Console.WriteLine("\n");

            methodBooks.name = "Идиот"; // Можем изменять напрямую
            methodBooks.autor = "Достоевский";
            Console.Write("После изменения второго класса без свойств: ");
            Console.WriteLine(methodBooks.Info());


        }
    }
}
