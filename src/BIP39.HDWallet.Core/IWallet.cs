using AElf.Types;

namespace BIP39.HDWallet.Core
{
    public interface IWallet
    {
        Address Address { get; }

        byte[] Sign(byte[] message);

        public byte[] PrivateKey { get; set; }

        public uint Index { get; set; }
    }
}