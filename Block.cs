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
        this.hash = null;
        this.timestamp = DateTime.Now.Ticks;
    }
    public string GetHash(string s) {return Encoding.ASCII.GetString(SHA256.HashData(Encoding.ASCII.GetBytes(s)));}
}