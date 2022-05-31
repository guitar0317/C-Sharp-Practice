// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System.Threading;
/*Thread t1 = new Thread(()=>ThreadClass.Background("J"));
t1.Start();
//t1.Join();
for(int i=0; i<500 ;i++){
            Console.Write("A");
        }*/
 WaitHandle[] AllWaitHandles = new WaitHandle[]
    {
        new AutoResetEvent(false),new AutoResetEvent(false)
    };
    ThreadPool.QueueUserWorkItem(ThreadClass.MyMethod23, AllWaitHandles[0]);
    ThreadPool.QueueUserWorkItem(ThreadClass.MyMethod24, AllWaitHandles[1]);
    WaitHandle.WaitAll(AllWaitHandles);
    Console.WriteLine();
    Console.WriteLine($"MyMethod23 的執行結果為 {ThreadClass.Result1}");
    Console.WriteLine($"MyMethod24 的執行結果為 {ThreadClass.Result2}");

class ThreadClass{
    private static int result1, result2;
    public ThreadClass(){}

    public static int Result1{
        get {return result1;}
        set {result1 = value;}

    }

      public static int Result2{
        get {return result2;}
        set {result2 = value;}

    }

    public static void Background(string str){
        for(int i=0; i<500 ;i++){
            Console.Write(str);
        }
    }

    public static void MyMethod23(object state){
        AutoResetEvent are = (AutoResetEvent)state;
        for (int i = 0; i < 800; i++)
        {
            Console.Write("*");
        }
        Result1 = 800;
        are.Set();
}
    public static void MyMethod24(object state){
        AutoResetEvent are = (AutoResetEvent)state;
        for (int i = 0; i < 500; i++)
        {
            Console.Write("-");
        }
        Result2 = 500;
        are.Set();
    }

}