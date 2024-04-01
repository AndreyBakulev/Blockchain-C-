public class Program
{
    public static void Main(string[] args)
    {
        Run();
    }

    public static void Run()
    {
        Blockchain bChain = null;
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("===========================================");
        Console.WriteLine("=====CPU Bitcoin miner by Andrey Bakulev===");
        Console.WriteLine("===========================================");
        Console.WriteLine("==================Options==================");
        Console.WriteLine("=========1.Create New Blockchain===========");
        Console.WriteLine("========2.Load Existing Blockchain=========");
        Console.ForegroundColor = ConsoleColor.White;
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("Input difficulty of new Blockchain");
                int difficulty = int.Parse(Console.ReadLine());
                bChain = new(difficulty);
                Console.WriteLine("Input number for Blockchain");
                bChain.SaveBlockchain(int.Parse(Console.ReadLine()));
                break;
            case "2":
                Console.WriteLine("Input number of Blockchain to load");
                bChain = Blockchain.LoadBlockchain(int.Parse(Console.ReadLine()));
                break;
            default:
                Console.WriteLine("Maybe type 1 or 2 next time monkey");
                break;
        }
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("===========================================");
        Console.WriteLine("=====CPU Bitcoin miner by Andrey Bakulev===");
        Console.WriteLine("===========================================");
        Console.WriteLine("==================Options==================");
        Console.WriteLine("==========1.Add block to chain=============");
        Console.WriteLine("======2.Verify Integrity of the chain======");
        Console.WriteLine("=======3.Retrieve info from a block========");
        Console.WriteLine("======4.Adjust the chain's difficulty======");
        Console.WriteLine("=============5.Delete a Block==============");
        Console.ForegroundColor = ConsoleColor.White;
        int choice2 = int.Parse(Console.ReadLine());
        switch (choice2)
        {
            case 1:
                Console.WriteLine("Select mining type: parallel (1), linear (2)");
                string selection = Console.ReadLine();
                Console.WriteLine("Validating chain...");
                if (bChain.ValidateChain())
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
                    bChain.MineLatest();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Input");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
                // Save the blockchain using binary serialization
                Console.WriteLine("Input number of blockchain");
                bChain.SaveBlockchain(int.Parse(Console.ReadLine()));
                Console.WriteLine("Saved blockchain!");
                break;
            case 2:
                if (bChain.ValidateChain())
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
                bChain.RetrieveBlock(index);
                break;
            case 4:
                // AdjustDifficulty();
                break;
            case 5:
                Console.WriteLine("Enter the index of the block to remove:");
                int removeIndex = Int32.Parse(Console.ReadLine());
                bChain.RemoveBlock(removeIndex);
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
0: single threaded running (DONE)
1: parallel running
2: parallel running with cuda (separate repo)
3: rust
4: tauri (netter)
*/