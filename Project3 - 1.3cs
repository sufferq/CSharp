namespace ConsoleApp2
{

    class Computer() {

        public string model = string.Empty;
        public string processorClockFrequency = string.Empty;
        public string RAM = string.Empty;
        public string hardDrive = string.Empty;

        public void Info() {

            Console.WriteLine("Инфо о пк: ");
            Console.WriteLine("Модель: " + model + ","
                + " Тактовая частота процессора: " + processorClockFrequency + ", "
                + "Объем оперативной памяти: " + RAM + ", "
                + "Объем жесткого диска: " + hardDrive);
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
          Computer computer = new Computer();

            computer.model = "ASUS ROG Strix G15CE";
            computer.processorClockFrequency = "3.8 ГГц";
            computer.RAM = "32 ГБ DDR4";
            computer.hardDrive = "1 ТБ SSD + 2 ТБ HDD";

            computer.Info();
            
        }
    }
}
