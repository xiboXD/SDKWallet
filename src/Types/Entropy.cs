namespace BIP39Wallet.Types
{
    public class Entropy
    {
        public string Hex { get; set; }
        public Language Language { get; set; }

        public Entropy()
        {
            
        }

        public Entropy(string hex, Language language = Language.English)
        {
            Hex = hex;
            Language = language;
        }

        public override string ToString()
        {
            return Hex;
        }
    }
}