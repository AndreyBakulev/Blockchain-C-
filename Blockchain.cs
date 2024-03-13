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
    public bool ValidateChain(){
        for(int i = 1; i < chain.Count; i++){
            Block currentBlock = chain.ElementAt(i);
            Block previousBlock = chain.ElementAt(i - 1);
            if(Block.CalculateHash(previousBlock.GetIndex() + previousBlock.GetPreviousHash()
            + previousBlock.GetData() + previousBlock.GetNonce()) != currentBlock.GetPreviousHash()){
                return false;
            }
        }
        return true;
    }
}