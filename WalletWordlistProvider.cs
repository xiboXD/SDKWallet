namespace Wallet
{
    public interface WalletWordlistProvider
    {
        string[] LoadWordlist(Language language);
    }
}