internal class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        int[] nums = {5,3,8,12,1,4};
        BubbleSort(nums);

        foreach(int i in nums) Console.Write(i + " ");
       
    }
    private static void BubbleSort(int[] nums){
        for(int round = 0 ; round < nums.Length; round++){
            /*for(int i_run= 0; i_run < nums.Length-1; i_run++){
                if(nums[i_run] > nums[i_run+1]){
                    Swap(nums, i_run, i_run+1); 
                }
            }*/
             int len = nums.Length - round; //imporve speed
             for(int i_run= 1; i_run < len; i_run++){
                if(nums[i_run-1] > nums[i_run]){
                    Swap(nums, i_run-1, i_run); 
                }
            }
        }
    }

    private static void Swap(int[] nums, int i_left, int i_right)
    {
        int tmep = nums[i_left];
        nums[i_left]= nums[i_right];
        nums[i_right] = tmep;
    }
}