using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
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
        genesisBlock.SetHash("0");
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
            if (Block.CalculateHash(previousBlock.GetIndex() + previousBlock.GetPreviousHash() + previousBlock.GetData() + currentBlock.GetNonce()) != currentBlock.GetPreviousHash())
            {
                return false;
            }
        }
        return true;
    }
    public void StartMiningParallel()
    {
        Block newBlock = GetLatestBlock();
        long nonce = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        string baseBlock = newBlock.GetIndex() + newBlock.GetPreviousHash() + newBlock.GetData();
        string correctString = new('0', difficulty);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Starting Mining on chain of {difficulty} difficulty");

        string hash = "";
        int totalThreads = Environment.ProcessorCount;
        bool hashFound = false;
        Parallel.For(0, totalThreads, (threadIndex) =>
        {
            long startNonce = threadIndex;
            long step = totalThreads;

            while (!hashFound)
            {
                string tempHash = Block.CalculateHash(baseBlock + startNonce);
                if (tempHash[..difficulty] == correctString)
                {
                    hash = tempHash;
                    nonce = startNonce;
                    hashFound = true;
                    break;
                }
                else
                {
                    Console.WriteLine($"#{startNonce}, Hash: {tempHash}");
                }

                startNonce += step;
            }
        });

        watch.Stop();
        double seconds = (double)watch.ElapsedMilliseconds / 1000;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Block Mined in {seconds} seconds! \nNonce: {nonce} \nHash: {hash}");
        Console.WriteLine("Please enter the data for a new Block");
        Block newBlock2 = new(Console.ReadLine(), GetLatestBlock());
        newBlock2.SetNonce(nonce);
        newBlock2.SetHash(hash.ToString());
        AddBlock(newBlock2);
        Console.ForegroundColor = ConsoleColor.White;
        times.Add(seconds);
        Console.WriteLine("To Mine the next node, type 1");
        if (Console.ReadLine() == "1")
        {
            StartMiningParallel();
        }
        else
        {
            double averageTime = 0;
            for (int i = 0; i < times.Count; i++)
            {
                averageTime += times[i];
            }
            Console.WriteLine($"Your average time to find a difficulty {difficulty} hash was {averageTime / times.Count} seconds");
            times.Clear();
            Console.WriteLine("Ok, Goodbye!");
        }

        using (StreamWriter outputFile = new StreamWriter("BlockchainLedger.txt"))
        {
            for (int i = 0; i < GetChain().Count; i++)
            {
                Block currentBlock = GetChain().ElementAt(i);
                outputFile.WriteLine(
                    $"Index: {currentBlock.GetIndex()}, Hash: {currentBlock.GetHash()}, Prev Hash: {currentBlock.GetPreviousHash()}, Nonce: {currentBlock.GetNonce()}, Data: {currentBlock.GetData()},");
            }
        }
    }
    public void StartMining()
    {
        Console.WriteLine("Please enter data for your new block");
        Block newBlock = new(Console.ReadLine(), GetLatestBlock());
        long nonce = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        string baseBlock = newBlock.GetIndex() + newBlock.GetPreviousHash() + newBlock.GetData();
        string correctString = new('0', difficulty);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Starting Mining on chain of {difficulty} difficulty");
        while (true)
        {
            string hash = Block.CalculateHash(baseBlock + nonce);
            //if the first difficulty characters are not the correct string
            if (hash[..difficulty] != correctString)
            {
                //Console.WriteLine($"#{nonce}, hash: {hash}");
                nonce++;
            }
            else
            {
                watch.Stop();
                double seconds = (double)watch.ElapsedMilliseconds / 1000;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Block Mined in {seconds} seconds without Parallelism! \nNonce: {nonce} \nHash: {hash}");
                Console.ForegroundColor = ConsoleColor.White;
                newBlock.SetNonce(nonce);
                newBlock.SetHash(hash.ToString());
                times.Add(seconds);
                break;
            }
        }
        AddBlock(newBlock);
        using (StreamWriter outputFile = new StreamWriter("BlockchainLedger.txt"))
        {
            for (int i = 0; i < GetChain().Count; i++)
            {
                Block currentBlock = GetChain().ElementAt(i);
                outputFile.WriteLine(
                    $"Index: {currentBlock.GetIndex()}, Hash: {currentBlock.GetHash()}, Prev Hash: {currentBlock.GetPreviousHash()}, Nonce: {currentBlock.GetNonce()}, Data: {currentBlock.GetData()},");
            }
        }
        Console.WriteLine("Keep mining? (Y/N)");
        if (Console.ReadLine() == "Y" || Console.ReadLine() == ""){
            StartMining();
        }
        else{
            double averageTime = 0;
            for (int i = 0; i < times.Count; i++){
                averageTime += times[i];
            }
            Console.WriteLine($"You found a difficulty {difficulty} hash every {averageTime / times.Count} seconds ({times.Count} times)");
            times.Clear();
            Console.WriteLine("Ok, Goodbye!");
        }
    }
    public void RetrieveBlock(int index){
        Block.PrintBlock(GetChain().ElementAt(index));
    }
}

/* CUDA stuff:
using CUDA;
using CUDA.Runtime;

public class BlockchainMiner
{
    [global]
    public static void HashKernel(byte[] data, int dataLength, int nonce, byte[] hash)
    {
        int index = cuda.blockIdx.x * cuda.blockDim.x + cuda.threadIdx.x;
        if (index < dataLength)
        {
            // Perform hashing calculation here
            // Example: hash[index] = data[index] ^ nonce;
        }
    }

    public static void StartMining()
    {
        // Prepare data for hashing
        byte[] data = // ...
        int dataLength = // ...
        int nonce = 0;

        // Allocate memory on GPU
        CUdeviceptr dataPtr = cuda.Malloc(data);
        CUdeviceptr hashPtr = cuda.Malloc(new byte[dataLength]);

        // Copy data to GPU
        cuda.CopyToDevice(dataPtr, data);

        // Launch kernel
        dim3 blockDim = new dim3(256);
        dim3 gridDim = new dim3((dataLength + blockDim.x - 1) / blockDim.x);
        cuda.Launch(blockDim, gridDim).HashKernel(dataPtr, dataLength, nonce, hashPtr);

        // Copy hash from GPU
        byte[] hash = new byte[dataLength];
        cuda.CopyToHost(hashPtr, hash);

        // Process the hash and perform mining logic
        // ...

        // Free GPU memory
        cuda.Free(dataPtr);
        cuda.Free(hashPtr);
    }
}



*/