using System.Security.Cryptography;
using System.Text;
public class Block
{
    public int difficulty { get; set; }
    public int index { get; set; }
    public long timestamp { get; set; }
    public string data { get; set; }
    public string previousHash { get; set; }
    public long nonce { get; set; }
    public Block(string data, Block previousBlock)
    {
        this.difficulty = previousBlock?.difficulty ?? 0;
        this.index = previousBlock?.index + 1 ?? 0;
        this.data = data;
        if (this.index > 0)
        {
            this.previousHash = CalculateHash(previousBlock.index + previousBlock.previousHash + previousBlock.data + previousBlock.nonce + previousBlock.timestamp);
        }
        else this.previousHash = "0";
        this.nonce = 0;
        this.timestamp = DateTime.Now.Ticks;
        
    }
    public void SetIndex(int index) { this.index = index; }
    public void SetNonce(long nonce) { this.nonce = nonce; }

    public static void PrintBlock(Block b)
    {
        Console.WriteLine("--------------------------------");
        Console.WriteLine("Index: " + b.index);
        Console.WriteLine("Timestamp: " + b.timestamp);
        Console.WriteLine("Data: " + b.data);
        Console.WriteLine("Previous Hash: " + b.previousHash);
        Console.WriteLine("Nonce: " + b.nonce);
        Console.WriteLine("--------------------------------");
    }
    public static string CalculateHash(string rawData)
    {
        // Create a SHA256
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}