namespace BIP39.HDWallet
{
    public class BIP39HDWallet : Core.HDWallet<xBIP39Wallet>
    {
        public BIP39HDWallet(string seed) : base(seed, BIP39HDWalletConstants.BIP39Path)
        {

        }
    }
}