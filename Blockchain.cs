public class Blockchain{
    private LinkedList<Block> chain = new LinkedList<Block>();
    private Block genesisBlock;
    private int difficulty;

    public Blockchain(int difficulty){
        this.difficulty = difficulty;
        this.genesisBlock = new Block("Genesis Block");
        this.chain.AddLast(genesisBlock);
    }

}