namespace Project3___1._4
{
    class Notebook() {

        public string model = string.Empty;
        public string processorClockFrequency = string.Empty;
        public string RAM = string.Empty;
        public string hardDrive = string.Empty;
        public string weight = string.Empty;

        public void Info() {

            Console.WriteLine("Инфо о ноутбуке: ");
            Console.WriteLine("Модель: " + model + ","
                + " Тактовая частота процессора: " + processorClockFrequency + ", "
                + "Объем оперативной памяти: " + RAM + ", "
                + "Объем жесткого диска: " + hardDrive
                + "Масса: " + weight);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Notebook notebook = new Notebook();

            notebook.model = "Apple MacBook Air M2";
            notebook.processorClockFrequency = "3.49 ГГц";
            notebook.RAM = "16 ГБ";
            notebook.hardDrive = "512 ГБ SSD";
            notebook.weight = "1.24 кг";

            notebook.Info();
        }
    }
}
