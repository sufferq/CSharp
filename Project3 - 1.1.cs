namespace ConsoleApp10
{

    class Point {
    
        public int x; public int y; public int z;

        public void MoveBy(int dx, int dy, int dz)
        {
            x = dx; y = dy; z = dz;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Point p = new Point();

            p.x = 2;
            p.y = 3;
            p.z = 4;

            Console.WriteLine("Изначальные координыты точек = X: " + p.x + ", Y: " + p.y + ", Z: " + p.z);

            p.MoveBy(10,20,30);

            Console.WriteLine("Измененные координыты точек = X: " + p.x + ", Y: " + p.y + ", Z: " + p.z);
        }
    }
}
