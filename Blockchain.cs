using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
public class Blockchain
{
    private LinkedList<Block> chain = new();
    private int difficulty;
    private List<double> times = [];
    public Blockchain(int difficulty)
    {
        this.difficulty = difficulty;
        this.createGenesisBlock();
    }
    public void createGenesisBlock()
    {
        Block genesisBlock = new("Genesis Block", null);
        chain.AddLast(genesisBlock);
        WriteToFile(genesisBlock);
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
    public LinkedList<Block> GetChain()
    {
        return chain;
    }
    public bool ValidateChain()
    {
        for (int i = 1; i < chain.Count; i++)
        {
            Block currentBlock = chain.ElementAt(i);
            Block previousBlock = chain.ElementAt(i - 1);
            if (Block.CalculateHash(previousBlock.GetIndex() + previousBlock.GetPreviousHash() + previousBlock.GetData() + previousBlock.GetNonce() + previousBlock.GetTimestamp()) != currentBlock.GetPreviousHash())
            {
                return false;
            }
        }
        return true;
    }
    public void StartMining()
    {
        //problem with genesis block bc its out of bounds
        //pass in currentblock as param
        Console.WriteLine("Please enter data for your new block");
        Block newBlock = new(Console.ReadLine(), GetLatestBlock());
        long nonce = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        StringBuilder baseBlockBuilder = new();
        baseBlockBuilder.Append(newBlock.GetIndex());
        baseBlockBuilder.Append(newBlock.GetPreviousHash());
        baseBlockBuilder.Append(newBlock.GetData());
        string baseBlock = baseBlockBuilder.ToString();
        string correctString = new('0', difficulty);
        int divisor = (int)Math.Pow(10, difficulty - 1) / 10;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Mining block #{GetLatestBlock().GetIndex()} of {difficulty} difficulty");
        while (true)
        {
            string hash = Block.CalculateHash(baseBlock + nonce); // Inline the hash calculation
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
        WriteToFile(newBlock);
        Console.WriteLine("Would you like to add a new block?(Y/N)");
        string choice = Console.ReadLine();
        if (choice.ToLower() == "y" || choice == "")
        {
            StartMining();
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
            SerializeBlockchain();
            Console.WriteLine("Goodbye!");
        }
    }
    public void RetrieveBlock(int index)
    {
        Block.PrintBlock(GetChain().ElementAt(index));
    }
    public void RemoveBlock(int index)
    {
        GetChain().Remove(GetChain().ElementAt(index));
        for (int i = index; i < GetChain().Count; i++)
        {
            //mine each block in front of the deleted ones
        }
    }
    public static void WriteToFile(Block newBlock)
    {
        string filePath = "blockchainLedger.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"Index: {newBlock.GetIndex()}, Prev Hash: {newBlock.GetPreviousHash()}, Nonce: {newBlock.GetNonce()}, Data: {newBlock.GetData()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while writing to the file: " + ex.Message);
        }
    }
    public void SerializeBlockchain()
    {
        string filePath = "blockchain.bin";
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fileStream))
        {
            writer.Write(chain.Count);
            foreach (Block block in chain)
            {
                writer.Write(block.GetIndex());
                writer.Write(block.GetTimestamp());
                writer.Write(block.GetData());
                writer.Write(block.GetPreviousHash());
                writer.Write(block.GetNonce());
            }
        }
    }

    public void DeserializeBlockchain()
    {
        //since im putting genesis block in maybe start i at 1 so i dont double copy?
        string filePath = "blockchain.bin";
        if (File.Exists(filePath))
        {
            chain.Clear();
            createGenesisBlock();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int index = reader.ReadInt32();
                    long timestamp = reader.ReadInt64();
                    string data = reader.ReadString();
                    string previousHash = reader.ReadString();
                    long nonce = reader.ReadInt64();
                    Block block = new Block(data, GetLatestBlock());
                    block.SetIndex(index);
                    block.SetNonce(nonce);
                    chain.AddLast(block);
                }
            }
        }
    }
}