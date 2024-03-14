using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
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
                writer.WriteLine($"Index: {newBlock.GetIndex()}, Prev Hash: {newBlock.GetPreviousHash()}, Nonce: {newBlock.GetNonce()}, Data: {newBlock.GetData()},");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while writing to the file: " + ex.Message);
        }
    }
}