using System;
using System.Text;
using Wallet;

namespace Wallet.Types
{
    public class Mnemonic
    {
        public string Value { get; set; }
        public Language Language { get; set; }

        public Mnemonic()
        {

        }

        public Mnemonic(string value, Language language = Language.English)
        {
            Value = value;
            Language = language;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}