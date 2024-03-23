using System.Text;
using Newtonsoft.Json;

public class Blockchain
{
    private LinkedList<Block> chain { get; set; }
    private int difficulty;
    private List<double> times = [];
    public Blockchain(int difficulty)
    {
        this.difficulty = difficulty;
        this.chain = new LinkedList<Block>();
        this.createGenesisBlock();
    }
    public void createGenesisBlock()
    {
        Block genesisBlock = new("Genesis Block", null)
        {
            difficulty = difficulty
        };
        chain.AddLast(genesisBlock);
    }
    public void AddBlock(Block b)
    {
        chain.AddLast(b);
    }

    public void removeBlock(Block b)
    {
        chain.Remove(b);
    }

    public Block GetLatestBlock()
    {
        return chain.Last.Value;
    }
    public bool ValidateChain()
    {
        for (int i = 1; i < chain.Count; i++)
        {
            Block currentBlock = chain.ElementAt(i);
            Block previousBlock = chain.ElementAt(i - 1);
            if (Block.CalculateHash(previousBlock.index + previousBlock.previousHash + previousBlock.data + previousBlock.nonce + previousBlock.timestamp) != currentBlock.previousHash)
            {
                return false;
            }
        }
        return true;
    }
    public void MineLatest()
    {
        Console.WriteLine("Please enter data for your new block");
        Block newBlock = new(Console.ReadLine(), GetLatestBlock());
        long nonce = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        StringBuilder baseBlockBuilder = new();
        baseBlockBuilder.Append(newBlock.index);
        baseBlockBuilder.Append(newBlock.previousHash);
        baseBlockBuilder.Append(newBlock.data);
        string baseBlock = baseBlockBuilder.ToString();
        string correctString = new('0', difficulty);
        int divisor = Math.Min((int)Math.Pow(10, difficulty) / 100, 10000);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Mining block #{newBlock.index} of {difficulty} difficulty");
        while (true)
        {
            string hash = Block.CalculateHash(baseBlock + nonce); // Inline the hash calculation?
            if (hash[..difficulty] == correctString)
            {
                watch.Stop();
                double seconds = (double)watch.ElapsedMilliseconds / 1000;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nBlock Mined in {seconds} seconds without Parallelism! \nNonce: {nonce} \nHash: {hash}");
                Console.ForegroundColor = ConsoleColor.White;
                newBlock.SetNonce(nonce);
                times.Add(seconds);
                break;
            }
            nonce++;
            if (nonce % divisor == 0)
            {
                Console.Write($"\r#{nonce}, Hash: {hash}");
            }
        }
        AddBlock(newBlock);
        WriteToFile(chain);
        Console.WriteLine("Would you like to add a new block?(Y/N)");
        string choice = Console.ReadLine();
        if (choice.ToLower() == "y" || choice == "")
        {
            MineLatest();
        }
        else
        {
            double averageTime = 0;
            for (int i = 0; i < times.Count; i++)
            {
                averageTime += times[i];
            }
            Console.WriteLine($"You found a difficulty {difficulty} hash every {averageTime / times.Count} seconds ({times.Count} times)");
            times.Clear();
            Console.WriteLine("Goodbye!");
        }
    }
    public void RetrieveBlock(int index)
    {
        Block.PrintBlock(chain.ElementAt(index));
    }
    public void RemoveBlock(int index)
    {
        chain.Remove(chain.ElementAt(index));
        RecalculateChain(index);
    }
    public static void WriteToFile(LinkedList<Block> c)
    {
        string filePath = "blockchainLedger.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < c.Count; i++)
                {
                    writer.WriteLine($"Index: {c.ElementAt(i).index}, Prev Hash: {c.ElementAt(i).previousHash}, Nonce: {c.ElementAt(i).nonce}, Data: {c.ElementAt(i).data}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while writing to the file: " + ex.Message);
        }
    }
    // Save the blockchain to a file
    public void SaveBlockchain(int num)
    {
        string json = JsonConvert.SerializeObject(chain);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Blockchain saved as #{num}!");
        Console.ForegroundColor = ConsoleColor.White;
        File.WriteAllText($"blockchain{num}.json", json);
    }
    // Load the blockchain from a file
    public static Blockchain LoadBlockchain(int num)
    {
        if (File.Exists($"blockchain{num}.json"))
        {
            string json = File.ReadAllText($"blockchain{num}.json");
            LinkedList<Block> test = JsonConvert.DeserializeObject<LinkedList<Block>>(json);
            Blockchain b = new(test.ElementAt(0).difficulty)
            {
                chain = test
            };
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Blockchain number {num} Loaded!");
            Console.ForegroundColor = ConsoleColor.White;
            return b;
        }
        else
        {
            Console.WriteLine($"Couldn't find blockchain #{num}");
            return null;
        }
    }
    public void RecalculateChain(int index)
    {
        //only thing you are remembering is the data
        Console.WriteLine($"Recalculating chain #{index}");
        Block newBlock = new(chain.ElementAt(index).data, chain.ElementAt(index - 1));
        long nonce = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        StringBuilder baseBlockBuilder = new();
        baseBlockBuilder.Append(newBlock.index);
        baseBlockBuilder.Append(newBlock.previousHash);
        baseBlockBuilder.Append(newBlock.data);
        string baseBlock = baseBlockBuilder.ToString();
        string correctString = new('0', difficulty);
        int divisor = Math.Min((int)Math.Pow(10, difficulty) / 100, 10000);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Mining block #{newBlock.index} of {difficulty} difficulty");
        while (true)
        {
            string hash = Block.CalculateHash(baseBlock + nonce); // Inline the hash calculation?
            if (hash[..difficulty] == correctString)
            {
                watch.Stop();
                double seconds = (double)watch.ElapsedMilliseconds / 1000;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nBlock Mined in {seconds} seconds without Parallelism! \nNonce: {nonce} \nResult: {hash}");
                Console.ForegroundColor = ConsoleColor.White;
                newBlock.SetNonce(nonce);
                times.Add(seconds);
                break;
            }
            nonce++;
            if (nonce % divisor == 0)
            {
                Console.Write($"\r#{nonce}, Hash: {hash}");
            }
        }
        chain.Remove(chain.ElementAt(index));
        chain.AddAfter(chain.Find(chain.ElementAt(index - 1)), newBlock);
        Console.WriteLine("Finished node! Continue? (Y/N)");
        Console.WriteLine(chain.Count);
        string choice = Console.ReadLine();
        if ((choice.ToLower() == "y" || choice == "") && (index + 1 != chain.Count))
        {
            RecalculateChain(index + 1);
        }
        else
        {
            double averageTime = 0;
            for (int i = 0; i < times.Count; i++)
            {
                averageTime += times[i];
            }
            WriteToFile(chain);
            Console.WriteLine($"You found a difficulty {difficulty} hash every {averageTime / times.Count} seconds ({times.Count} times)");
            Console.WriteLine($"Finished at index: {index}");
            times.Clear();
            Console.WriteLine("Goodbye!");
        }
    }
}