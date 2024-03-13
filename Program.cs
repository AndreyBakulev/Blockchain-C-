using System;
public class Program
{
    public static void Main(string[] args)
    {
        Blockchain blockchain = new(2);
        blockchain.createGenesisBlock();
        Block.PrintBlock(blockchain.GetLatestBlock());
        blockchain.AddBlock(new Block("Block 1",blockchain.GetLatestBlock()));
        Block.PrintBlock(blockchain.GetLatestBlock());
    }
}