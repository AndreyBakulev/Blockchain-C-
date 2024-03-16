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
        Console.WriteLine("Select a difficulty of a block");
        int difficulty = Int32.Parse(Console.ReadLine());
        Blockchain blockchain = new(difficulty);
        // Check if the blockchain data exists
        if (File.Exists("blockchain.json"))
        {
            blockchain = blockchain.LoadBlockchain(difficulty);
        } else Console.WriteLine("Made new Blockchain");

        switch (choice)
        {
            case 1:
                Console.WriteLine("Select mining type: parallel (1), linear (2)");
                string selection = Console.ReadLine();
                Console.WriteLine("Validating chain...");
                if (blockchain.ValidateChain())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Chain is valid!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chain is not valid!");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                if (selection == "1")
                {
                    // blockchain.StartMiningParallel();
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
                // Save the blockchain using binary serialization
                Blockchain.SaveBlockchain(blockchain);
                break;
            case 2:
                if (blockchain.ValidateChain())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Chain is valid!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chain invalid!");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                break;
            case 3:
                // Load the existing blockchain
                Console.WriteLine("Enter the index of the block to retrieve:");
                int index = Int32.Parse(Console.ReadLine());
                blockchain.RetrieveBlock(index);
                break;
            case 4:
                // AdjustDifficulty();
                break;
            case 5:
                Console.WriteLine("Enter the index of the block to remove:");
                int removeIndex = Int32.Parse(Console.ReadLine());
                blockchain.RemoveBlock(removeIndex);
                break;
            default:
                Console.WriteLine("Invalid choice, please try again");
                break;
        }
    }
}
/*
Notes:
TODO{
    decide between db or serialization
    build base req
}
LEVELS:
0: single threaded running 
1: parallel running
2: parallel running with cuda (separate repo)
3: rust
*/