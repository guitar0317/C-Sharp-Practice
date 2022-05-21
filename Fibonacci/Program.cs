// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
for(int i=0; i<20; i++){
    Console.Write(Fobinacci.calc(i) + " ");
}

class Fobinacci{
    public Fobinacci(){}
    public static int calc(int n){
        if(n==0) return 0;
        if(n==1) return 1;

        return Fobinacci.calc(n-2) + Fobinacci.calc(n-1);
    }
}