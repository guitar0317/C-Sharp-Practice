//Console.WriteLine("Hello, World!");

 class Program
{
    private static void Main(string[] args)
    {
        int a = 8, b = 2, c=3;
        string s="a";
        //Console.WriteLine("Hello, World!");
        switch(s){
            case "a":
                if(a>b){
                    if(a<c) break;
                    else if(c<b) break;
                    for(int i=0; i<10; i++){
                         Console.WriteLine(i);
                        if(i==b) break;
                    }
                }
                Console.WriteLine("a");
                break;
            default:
                break;
        }
        Console.WriteLine("Hello, World!");
    }
}