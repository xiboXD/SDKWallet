namespace SDK.HDWallet
{
    public class SDKHDWallet : Core.HDWallet<SDKWallet>
    {
        public SDKHDWallet(string seed) : base(seed, SDKHDWalletConstants.SDKPath)
        {

        }
    }
}