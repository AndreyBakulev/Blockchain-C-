public class Block{
    private int index;
    private long timestamp;
    private string data;
    private string previousHash;
    private string hash;
    private long nonce;
    public Block(string data){
        this.data = data;
        this.nonce = 0;
        this.hash = Encoding.ASCII.GetString(Encrypt(Encoding.ASCII.GetBytes()));
        this.timestamp = DateTime.Now.Ticks;
    }



    public byte[] Encrypt(byte[] data)
        {
            byte[] encryptedData = null;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                encryptedData = sha256Hash.ComputeHash(data);
            }
            return encryptedData;
        }
}