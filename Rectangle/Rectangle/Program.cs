using System;

namespace RectangleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Rectangle r1 = new Rectangle(4, 3);
            Rectangle r2 = new Rectangle(5.2, 3);
            //r.Acceptdetails();
            //Jared brancj
            r1.Display();
            //r2.Display();
            Console.ReadKey();
        }
    }

    class Rectangle
    {
        // 成員變量
        double length;
        double width;
        public Rectangle(double l,double w)
        {
            this.length = l;
            this.width = w;
        }
      
        public void Acceptdetails()
        {
            length = 4.5;
            width = 3.5;
        }
        public double GetArea()
        {
            return Math.Round(length * width);
        }
        public void Display()
        {
            Console.WriteLine("Length: {0}", length);
            Console.WriteLine("Width: {0}", width);
            Console.WriteLine("Area: {0}", GetArea());
        }
    }
}
