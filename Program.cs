public class Program
{
    public static void Main(string[] args)
    {
        Blockchain blockchain = new(4);
        blockchain.StartMining();
        using (StreamWriter outputFile = new StreamWriter("BlockchainLedger.txt"))
        {
            for (int i = 0; i < blockchain.GetChain().Count; i++)
            {
                Block currentBlock = blockchain.GetChain().ElementAt(i);
                outputFile.WriteLine(
                    $"Index: {currentBlock.GetIndex()}, Hash: {currentBlock.GetHash()}, Prev Hash: {currentBlock.GetPreviousHash()}, Nonce: {currentBlock.GetNonce()}, Data: {currentBlock.GetData()},");
            }
        }
    }
}
/*
Notes:
validateChain isnt working bc idk

ADDITIONS{
    make this with cuda!
}

*/