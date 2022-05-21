using System;

namespace helloworld{
    class Program{
        public static void Main(string[] args){
            Console.WriteLine("Hello, World!");
            string? s = Console.ReadLine();
            Console.WriteLine("Your input:"+s.Trim());
        }
    }
}

