using System.Security.Cryptography;
using System.Text;
public class Block{
    private int index;
    private long timestamp;
    private string data;
    private string previousHash;
    private string hash;
    private long nonce;
    public Block(string data){
        //this.index = previousBlock.index;
        this.data = data;
        //this.previousHash = previousBlock.hash; 
        this.nonce = 0;
        this.hash = GetHash(index+previousHash+data+nonce);
        this.timestamp = DateTime.Now.Ticks;
    }
    public static string GetHash(string s) {return Encoding.ASCII.GetString(SHA256.HashData(Encoding.ASCII.GetBytes(s)));}
}