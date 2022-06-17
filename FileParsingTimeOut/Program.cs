// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System.IO;
string location = System.Environment.CurrentDirectory;
Console.WriteLine($"Current path: {location}");
int line=1;
Console.WriteLine("Please input file name:");
string filename = Console.ReadLine();
string Outputfile = location+@"\"+filename+"-"+DateTime.Now.ToString("MM-dd-HH-mm-ss")+".txt";
Console.WriteLine("Please input time out limit(sec):");
double timeout = double.Parse(Console.ReadLine());
filename = location+@"\"+filename+".log";
//double timeout = 2.0f;
string last=null, current=null;
//string temp = null;
DateTime CurrentDT, LastDT;
TimeSpan diff;

StreamWriter writer = new StreamWriter(Outputfile);
writer.WriteLine($"Timeout limit = {timeout}");
writer.WriteLine("----------------------------------");
writer.WriteLine();
writer.WriteLine();

foreach (string s in File.ReadLines(filename))
{  
    if(string.IsNullOrEmpty(last)){
         last = s;
    }else{
        try{
            current = s;  
            //temp = current.Split(" : ");
            CurrentDT = DateTime.ParseExact(current.Split(" : ")[0],"MM/dd/yyyy HH:mm:ss:fffffffK",null);
            LastDT = DateTime.ParseExact(last.Split(" : ")[0],"MM/dd/yyyy HH:mm:ss:fffffffK",null);
            diff = CurrentDT.Subtract(LastDT);
            if(timeout <=  diff.TotalSeconds){
                Console.WriteLine($"{line-1}: {last}");
                writer.WriteLine($"{line-1}: {last}");
                Console.WriteLine($"{line}: {current}");
                writer.WriteLine($"{line}: {current}");
                Console.WriteLine($"Diff = {diff.TotalSeconds}");
                writer.WriteLine($"Diff = {diff.TotalSeconds}");
                Console.WriteLine("-----------------------------------------------------");
                writer.WriteLine("-----------------------------------------------------");
            }
            last = s;
        }catch(Exception e){
            //Console.WriteLine($"Exception:{line}  {e.Message}");
        }  
    }
    line++;  
}  
writer.Close();
Console.WriteLine($"Total line {line}");
Console.WriteLine($"Output file location: {Outputfile}");
System.Console.ReadLine();
