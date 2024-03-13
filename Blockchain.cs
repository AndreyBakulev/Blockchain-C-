public class Blockchain{
    private LinkedList<Block> chain = new();
    private int difficulty;

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
        //make a new block then mine it until it fits difficulty then return
        Block newBlock = new($"block {GetChain().Count}", GetLatestBlock());
        newBlock = GetLatestBlock();
        AddBlock(newBlock);
        long nonce = 0;
        int difficulty = this.difficulty;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        string baseBlock = newBlock.GetIndex()+newBlock.GetPreviousHash()+newBlock.GetData();
        string correctString = new string('0',difficulty);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Starting Mining on chain of {difficulty} difficulty");
        while(true){
            string hash = Block.CalculateHash(baseBlock+nonce);
            if(hash[..difficulty] != correctString){
                Console.WriteLine($"Tested Nonce: {nonce} and got {hash}");
                nonce++;
            } else {
                watch.Stop();
                double seconds = (double) watch.ElapsedMilliseconds/1000;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Block Mined in {seconds} seconds! \nNonce: {nonce} \nHash: {hash}");
                newBlock.SetNonce(nonce);
                newBlock.SetHash(hash.ToString());
                Block newBlock2 = new($"block {GetChain().Count}", GetLatestBlock());
                AddBlock(newBlock2);
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }
        }
    }
}