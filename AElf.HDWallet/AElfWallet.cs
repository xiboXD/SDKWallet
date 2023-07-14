using AElf.HDWallet.Core;
using AElf.Types;

namespace AElf.HDWallet
{
    public class AElfWallet : Wallets
    {
        protected override Address GenerateAddress()
        {
            return Address.FromPublicKey(Key.PubKey.ToBytes());
        }
    }
}