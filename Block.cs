using System.Security.Cryptography;
using System.Text;
public class Block{
    private int index;
    private long timestamp;
    private string data;
    private string previousHash;
    private string hash;
    private long nonce;
      public Block(string data, Block previousBlock)
    {
        this.index = previousBlock?.GetIndex() + 1 ?? 0;
        this.data = data;
        this.previousHash = previousBlock?.GetHash() ?? "0";
        this.nonce = 0;
        this.hash = CalculateHash(index + previousHash + data + nonce);
        this.timestamp = DateTime.Now.Ticks;
    }
    public int GetIndex(){return index;}
    public long GetTimestamp(){return timestamp;}
    public string GetData(){return data;}
    public string GetPreviousHash(){return previousHash;}
    public string GetHash(){return hash;}
    public long GetNonce(){return nonce;}
    public void SetIndex(int index){this.index = index;}
    public void SetHash(string hash){this.hash = hash;} 
    public static string CalculateHash(string s) {return Encoding.ASCII.GetString(SHA256.HashData(Encoding.ASCII.GetBytes(s)));}
    public static void PrintBlock(Block b){
        Console.WriteLine("--------------------------------");
        Console.WriteLine("Index: " + b.index);
        Console.WriteLine("Timestamp: " + b.timestamp);
        Console.WriteLine("Data: " + b.data);
        Console.WriteLine("Previous Hash: " + b.previousHash);
        Console.WriteLine("Nonce: " + b.nonce);
        Console.WriteLine("Hash: " + b.hash);
        Console.WriteLine("--------------------------------");
    }
}