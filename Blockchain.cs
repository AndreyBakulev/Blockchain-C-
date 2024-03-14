using System.Collections;
public class Blockchain{
    private LinkedList<Block> chain = new();
    private int difficulty;
    private List<double> times = [];

    public Blockchain(int difficulty){
        this.difficulty = difficulty;
        this.createGenesisBlock();
        }
    public void createGenesisBlock(){
        Block genesisBlock = new("Genesis Block", null);
        genesisBlock.SetHash("0");
        chain.AddLast(genesisBlock);
    }
    public void AddBlock(Block b){
        chain.AddLast(b);
    }
    public void removeBlock(Block b){
        chain.Remove(b);
    }
    public Block GetLatestBlock(){
        return chain.Last.Value;
    }
    public LinkedList<Block> GetChain(){
        return chain;
    }
    public bool ValidateChain(){
        for(int i = 1; i < chain.Count; i++){
            Block currentBlock = chain.ElementAt(i);
            Block previousBlock = chain.ElementAt(i - 1);
            if(Block.CalculateHash(previousBlock.GetIndex() + previousBlock.GetPreviousHash()
            + previousBlock.GetData() + currentBlock.GetNonce()) != currentBlock.GetPreviousHash()){
                return false;
            }
        }
        return true;
    }
    public void StartMining(){
        Block newBlock = GetLatestBlock();
        long nonce = 0;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        string baseBlock = newBlock.GetIndex()+newBlock.GetPreviousHash()+newBlock.GetData();
        string correctString = new('0',difficulty);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Starting Mining on chain of {difficulty} difficulty");
        while(true){
            string hash = Block.CalculateHash(baseBlock+nonce);
            //if the first difficulty characters are not the correct string
            if(hash[..difficulty] != correctString){
                Console.WriteLine($"#{nonce}, hash: {hash}");
                nonce++;
            } else {
                watch.Stop();
                double seconds = (double) watch.ElapsedMilliseconds/1000;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Block Mined in {seconds} seconds! \nNonce: {nonce} \nHash: {hash}");
                Console.WriteLine("Please enter the data for a new Block");
                Block newBlock2 = new(Console.ReadLine(), GetLatestBlock());
                newBlock2.SetNonce(nonce);
                newBlock2.SetHash(hash.ToString());
                AddBlock(newBlock2);
                Console.ForegroundColor = ConsoleColor.White;
                times.Add(seconds);
                break;
            }
        }
        Console.WriteLine("To Mine the next node, type 1");
        if(Console.ReadLine() == "1"){
            StartMining();
        } else {
            double averageTime = 0;
            for(int i = 0; i < times.Count; i++) {
                averageTime += times[i];
            }
            Console.WriteLine($"Your average time to find a difficulty {difficulty} hash was {averageTime/times.Count} seconds");
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
}