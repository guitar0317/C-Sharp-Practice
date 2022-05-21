// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
//Node star;
List l = new List();
l.add(1);
l.add(2);
l.print();
Console.WriteLine("\n-------------");
l.add(3);
l.add(4);
l.print();
Console.WriteLine("\n-------------");
int? val1 = l.search(4);
Console.Write(val1);
Console.WriteLine("\n-------------");
l.remove(4);
l.print();

class Node{
    public int value;
    public Node next;

    public Node(int val){
        this.value = val;
    } 
}

class List{
    private Node start, end;
    public void add(int val){
        if(start == null){
            start = new Node(val);
            end = start;
        }else{
            end.next = new Node(val);
            end = end.next;
        }
    }

    public int? search(int val){
        if(start == null) return null;

        Node node  = start;
        while(true){
            if(node ==null) return null;
            if(node.value == val) return node.value;
            node = node.next;
        }
        return null;
    }

    public void remove(int val){
        Node node =start;
        Node node_target = null;
        Node node_prev =null;
        Node node_r = null;
        //search to find node position
        while(true){
            if(node == null) break;
            if(node.value == val){
                node_target = node;
                node_r = node.next;
                break;
            }
            node_prev = node;
            node = node.next;
        }

        if(node_target ==null) return;
        
        if(node_target == start) start =node_r; //directlly cutoff linklist
        else{
            node_prev.next = node_target.next;
        }
    }

    public void print(){
           Node node =start;
           //Node node2 =node;
       while(true){
            if(node == null) break;
            Console.Write(node.value+",");
            node = node.next;
        }
    }
}