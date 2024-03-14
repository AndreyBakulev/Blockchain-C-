public class Program
{
    public static void Main(string[] args)
    {
        Blockchain blockchain = new(1);
        Console.WriteLine("Welcome to Andrey's Bitcoin Miner!");
        Console.WriteLine("For Parrallelism, select 1, and without select 2");
        string choice = Console.ReadLine();
        if(choice == "1"){
            blockchain.StartMiningParallel();
        } else if(choice == "2"){
            blockchain.StartMining();
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }
        
    }
}
/*
Notes:
validateChain isnt working bc idk

ADDITIONS{
    make this with cuda!
}

*/