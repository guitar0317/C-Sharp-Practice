// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System.IO;
internal class Program
{
    private static void Main(string[] args)
    {
        string location = System.Environment.CurrentDirectory;
        Console.WriteLine($"Current path: {location}");
        int line = 1;
        Console.WriteLine("Please input file name:");
        string filename = Console.ReadLine();
        bool ICParse = false;
        if(filename.Contains("IC")){
            ICParse = true;
        }

        string Outputfile = location + @"\" + filename + "-" + DateTime.Now.ToString("MM-dd-HH-mm-ss") + ".txt";
        Console.WriteLine("Please input time out limit(sec):");
        double timeout = double.Parse(Console.ReadLine());
        filename = location + @"\" + filename + ".log";
        //double timeout = 2.0f;
        string last = null, current = null;
        //string temp = null;
        DateTime CurrentDT, LastDT;
        TimeSpan diff;
        double dTotalDiffSec=0.0d;
        int iDiffCount=0;
  

        StreamWriter writer = new StreamWriter(Outputfile);
        writer.WriteLine($"Timeout limit = {timeout}");
        writer.WriteLine("----------------------------------");
        writer.WriteLine();
        writer.WriteLine();

        string DTformat = "MM/dd/yyyy HH:mm:ss:fffffffK";
        if(ICParse) DTformat="yyyy-MM-dd HH:mm:ss:fff";

        foreach (string s in File.ReadLines(filename))
        {
            if (string.IsNullOrEmpty(last))
            {
                last = s;
            }
            else
            {
                try
                {
                    current = s;
                    //temp = current.Split(" : ");
                    //CurrentDT = DateTime.ParseExact(current.Split(" : ")[0], DTformat, null);
                    //LastDT = DateTime.ParseExact(last.Split(" : ")[0], DTformat, null);
                    CurrentDT = DateTime.ParseExact(ParseDT(current,ICParse), DTformat, null);
                    LastDT = DateTime.ParseExact(ParseDT(last,ICParse), DTformat, null);
                    diff = CurrentDT.Subtract(LastDT);
                    if (timeout <= diff.TotalSeconds)
                    {
                        dTotalDiffSec+=diff.TotalSeconds;
                        iDiffCount++;
                        Console.WriteLine($"{line - 1}: {last}");
                        writer.WriteLine($"{line - 1}: {last}");
                        Console.WriteLine($"{line}: {current}");
                        writer.WriteLine($"{line}: {current}");
                        Console.WriteLine($"Diff = {diff.TotalSeconds}");
                        writer.WriteLine($"Diff = {diff.TotalSeconds}");
                        Console.WriteLine("-----------------------------------------------------");
                        writer.WriteLine("-----------------------------------------------------");
                    }
                    last = s;
                }
                catch (Exception e)
                {
                    //Console.WriteLine($"Exception:{line}  {e.Message}");
                }
            }
            line++;
        }

        Console.WriteLine($"Total line {line}");
        writer.WriteLine($"Total line {line}");
        Console.WriteLine($"Output file location: {Outputfile}");
        Console.WriteLine($"Difference Count = {iDiffCount}");
        writer.WriteLine($"Difference Count = {iDiffCount}");
        Console.WriteLine($"Total difference time(sec) = {dTotalDiffSec}");
        writer.WriteLine($"Total difference time(sec) = {dTotalDiffSec}");
        Console.WriteLine($"Total difference time(min) = {dTotalDiffSec/60}");
        writer.WriteLine($"Total difference time(min) = {dTotalDiffSec/60}");
        writer.Close();
        System.Console.ReadLine();
    }

    public static string ParseDT(string s, bool IC0)
    {
        string[] strtemp=null;        
        if(IC0){
            strtemp = s.Split(new char[]{'[',']'});
            return strtemp[1];
        }else{
             return s.Split(" : ")[0];
        }
    }
}