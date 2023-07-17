namespace Wallet
{
    public class WalletData
    {
        public string mnemonic { get; set; }
        public string rootSeed { get; set; }
        public string hdWallet { get; set; }
        public string childWallet { get; set; }
        public KeyPair keyPair { get; set; }
    }
    public class KeyPair
    {
        public string privateKey { get; set; }
        public string publicKey { get; set; }
    }
}