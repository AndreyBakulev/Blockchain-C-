public class Blockchain{
    private LinkedList<Block> chain = new();
    private int difficulty;

    public Blockchain(int difficulty){
        this.difficulty = difficulty;
        this.createGenesisBlock();
        }
    public void createGenesisBlock(){
        Block genesisBlock = new("Genesis Block", null);
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
}