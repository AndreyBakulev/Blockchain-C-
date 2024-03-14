public class Program
{
    public static void Main(string[] args)
    {
        Run();
    }

    public static void Run()
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("===========================================");
        Console.WriteLine("=====Bitcoin CPU miner by Andrey Bakulev===");
        Console.WriteLine("===========================================");
        Console.WriteLine("==================Options==================");
        Console.WriteLine("==========1.Add block to chain=============");
        Console.WriteLine("======2.Verify Integrity of the chain======");
        Console.WriteLine("=======3.Retrieve info from a block========");
        Console.WriteLine("======4.Adjust the chain's difficulty======");
        Console.WriteLine("=============5.Delete a Block==============");
        Console.ForegroundColor = ConsoleColor.White;
        int choice = Int32.Parse(Console.ReadLine());
        switch (choice)
        {
            case 1:
                Console.WriteLine("Please select difficulty of chain");
                int difficulty = Int32.Parse(Console.ReadLine());
                Blockchain blockchain = new(difficulty);
                Console.WriteLine("Select mining type: parallel (1), linear (2)");
                string selection = Console.ReadLine();
                if (selection == "1")
                {
                    blockchain.StartMiningParallel();
                }
                else if (selection == "2")
                {
                    blockchain.StartMining();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
                break;
            case 2:
                //ValidateChain();
                break;
            case 3:
                //RetrieveBlock(Int32.Parse(Console.ReadLine()));
                break;
            case 4:
                //AdjustDifficulty
                break;
            case 5:
                //RemoveBlock()
                break;
            default:
                Console.WriteLine("Invalid choice, please try again");
                break;
        }
    }
}
/*
Notes:
validateChain isnt working bc idk
ADDITIONS{
    make this with cuda!
}
LEVELS:
0: single threaded running
1: parallel running
2: parallel running with cuda
3: rust
*/