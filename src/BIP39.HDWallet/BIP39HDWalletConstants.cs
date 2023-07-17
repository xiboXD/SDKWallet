namespace BIP39.HDWallet
{
    public class BIP39HDWalletConstants
    {
        public const uint HardenedOffset = 0x80000000;
        /// <summary>
        /// m / purpose' / coin_type' / account' / change / address_index
        /// purpose: always 44
        /// coin_type: AELF is 1616 (https://github.com/satoshilabs/slips/blob/master/slip-0044.md)
        /// 
        /// </summary>
        public const string BIP39Path = "'m/44\'/1616\'/0\'/0/0'";
    }
}