using SDK.HDWallet.Core;
using AElf.Types;

namespace SDK.HDWallet
{
    public class SDKWallet : Wallets
    {
        protected override Address GenerateAddress()
        {
            return Address.FromPublicKey(Key.PubKey.ToBytes());
        }
    }
}