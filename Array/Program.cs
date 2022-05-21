// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
Array a = new Array(5);
a.add_by_value(1);
a.add_by_value(2);
a.add_by_value(3);
a.add_by_value(4);
a.add_by_value(5);
a.PrintArray();
Console.WriteLine("\n--------------");
a.add_by_value(6);
a.PrintArray();
Console.WriteLine("\n--------------");
a.add_by_index(2,10);
a.PrintArray();
Console.WriteLine("\n--------------");
int? val1= a.search_by_index(2);
int? val2 = a.search_by_value(5);
Console.WriteLine(val1);
Console.WriteLine(val2);
Console.WriteLine("--------------");
a.remove_by_value(2);
a.PrintArray();
Console.WriteLine("\n--------------");
a.remove_by_indedx(4);
a.PrintArray();
Console.WriteLine("\n--------------");


class Array{
    private int?[] array;
    private int i_end;

    public Array(int size){
        array = new int?[size];
        i_end=-1;
    }

    public void add_by_index(int i_add, int val){
        if(i_end+1 == array.Length) expand_space();
        if(i_add > i_end+1 || i_add < 0) return;

        //Each element move right direction
        for(int i=i_end; i >= i_add; i--){
            array[i+1] = array[i];
            array[i]= null;
        }
        array[i_add] = val;
        i_end++;
    }

    public void add_by_value(int val){
        add_by_index(i_end+1, val);
    }

    private void expand_space(){
        int?[] array_new = new int?[array.Length*2];
        for(int i=0; i<array.Length; i++){
            array_new[i] = array[i];
        }
        this.array = array_new;
    }

    public int? search_by_index(int index){
        if(index > i_end || index <0) return null;
        return array[index];
    }

    public int? search_by_value(int val){
        foreach(int? i in array){
            if(i == val) return i;
        }
        return null;
    }

    public void remove_by_indedx(int index){
        if(index > i_end || index < 0) return;
        array[index] = null;

        for(int i=index+1 ; i<=i_end ; i++){
            array[i-1] = array[i];
            array[i] = null;
        }

        i_end--;
    }

    public void remove_by_value(int val){
        for(int i=0; i<=i_end ;i++){
            if(array[i] == val){
                remove_by_indedx(i);
                return;
            }
        }
    }

    public void PrintArray(){
        foreach(int? i in array){
            if(i == null) break;
             Console.Write(i+", ");
             }
    }
}