using BIP39.HDWallet.Core;
using AElf.Types;

namespace BIP39.HDWallet
{
    public class xBIP39Wallet : Wallets
    {
        protected override Address GenerateAddress()
        {
            return Address.FromPublicKey(Key.PubKey.ToBytes());
        }
    }
}