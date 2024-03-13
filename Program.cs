public class Program
{
    public static void Main(string[] args)
    {
        Blockchain blockchain = new(2);
        blockchain.AddBlock(new Block("Block 1", blockchain.GetLatestBlock()));
        blockchain.AddBlock(new Block("Block 2", blockchain.GetLatestBlock()));
        blockchain.AddBlock(new Block("Block 3", blockchain.GetLatestBlock()));
        using (StreamWriter outputFile = new StreamWriter("BlockchainLedger.txt"))
        {
            for (int i = 0; i < blockchain.GetChain().Count; i++)
            {
                Block currentBlock = blockchain.GetChain().ElementAt(i);
                outputFile.WriteLine(
                    $"Index: {currentBlock.GetIndex()}, Data: {currentBlock.GetData()}, Hash: {currentBlock.GetHash()}, Nonce: {currentBlock.GetNonce()}");
            }
        }
        //Block.PrintBlock(blockchain.GetChain().ElementAt(0));
    }
}