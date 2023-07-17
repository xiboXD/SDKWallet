namespace BIP39Wallet
{
    public interface WalletWordlistProvider
    {
        string[] LoadWordlist(Language language);
    }
}