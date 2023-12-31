namespace BIP39Wallet.Extensions
{
    public static class StringExtensions
    {
        public static string LeftPad(this string str, string leftPadString, int length)
        {
            while (str.Length < length)
            {
                str = $"{leftPadString}{str}";
            }

            return str;
        }
    }
}