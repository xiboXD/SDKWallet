#pragma warning disable CS0618 // Type or member is obsolete
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Wallet.Types;
using Wallet.Extensions;
using System.Text.RegularExpressions;
using SDK.HDWallet;
using SDK.HDWallet.Core;
using AElf.Cryptography;
using AElf;



namespace Wallet
{
    public class Wallet
    {
    private readonly WalletWordlistProvider _wordlistProvider;

    public class BlockchainWallet
    {
        public string Address { get; private set; }
        public string PrivateKey { get; private set; }
        public string Mnemonic { get; private set; }
        public string PublicKey { get; private set; }

    public BlockchainWallet(string address, string privateKey, string mnemonic, string publicKey)
    {
        Address = address;
        PrivateKey = privateKey;
        Mnemonic = mnemonic;
        PublicKey = publicKey;
    }
    }
    public Mnemonic GenerateMnemonic(int strength, Language language)
    {
        if (strength % 32 != 0)
        {
            throw new NotSupportedException(WalletConstants.InvalidEntropy);
        }

        var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var buffer = new byte[strength / 8];
        rngCryptoServiceProvider.GetBytes(buffer);
        var entropy = new Entropy(BitConverter.ToString(buffer).Replace("-", ""), language);
        return ConvertEntropyToMnemonic(entropy);
    }
    public string[] LoadWordlist(Language language)
    {
        var file = language switch
        {
            Language.English => "english",
            Language.Japanese => "japanese",
            Language.Korean => "korean",
            Language.Spanish => "spanish",
            Language.ChineseSimplified => "chinese_simplified",
            Language.ChineseTraditional => "chinese_traditional",
            Language.French => "french",
            Language.Italian => "italian",
            Language.Czech => "czech",
            Language.Portuguese => "portuguese",
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

        file = $"{file}.txt";
        return File.ReadAllLines(GetPath(file));
    }

    private string GetPath(string fileName)
    {
        return $"../../../../src/Wordlists/{fileName}";
    }

    public Mnemonic ConvertEntropyToMnemonic(Entropy entropy)
    {
        var wordlist = LoadWordlist(entropy.Language);

        var entropyBytes = Enumerable.Range(0, entropy.Hex.Length / 2)
            .Select(x => Convert.ToByte(entropy.Hex.Substring(x * 2, 2), 16))
            .ToArray();
        var entropyBits = entropyBytes.ToBinary();
        var checksumBits = entropyBytes.GetChecksumBits();
        var bits = $"{entropyBits}{checksumBits}";
        var chunks = Regex.Matches(bits, "(.{1,11})")
            .Select(m => m.Groups[0].Value)
            .ToArray();

        var words = chunks.Select(binary =>
        {
            var index = Convert.ToInt32(binary, 2);
            return wordlist[index];
        });

        var joinedText = string.Join(entropy.Language == Language.Japanese ? "\u3000" : " ", words);

        return new Mnemonic(joinedText, entropy.Language);
    }
        public Entropy ConvertMnemonicToEntropy(Mnemonic mnemonic)
    {
        var wordlist = _wordlistProvider.LoadWordlist(mnemonic.Language);
        var words = mnemonic.Value.Normalize(NormalizationForm.FormKD).Split(new[] {' '},
            StringSplitOptions.RemoveEmptyEntries);

        if (words.Length % 3 != 0)
        {
            throw new FormatException(WalletConstants.InvalidMnemonic);
        }

        var bits = string.Join("", words.Select(word =>
        {
            var index = Array.IndexOf(wordlist, word);
            if (index == -1)
            {
                throw new FormatException(WalletConstants.InvalidMnemonic);
            }

            return Convert.ToString(index, 2).LeftPad("0", 11);
        }));

        var dividerIndex = (int) Math.Floor((double) bits.Length / 33) * 32;
        var entropyBits = bits.Substring(0, dividerIndex);
        var checksumBits = bits.Substring(dividerIndex);

        var entropyBytesMatch = Regex.Matches(entropyBits, "(.{1,8})")
            .Select(m => m.Groups[0].Value)
            .ToArray();

        var entropyBytes = entropyBytesMatch
            .Select(bytes => Convert.ToByte(bytes, 2)).ToArray();

        var newChecksum = entropyBytes.GetChecksumBits();

        if (newChecksum != checksumBits)
            throw new Exception(WalletConstants.InvalidChecksum);

        return new Entropy(BitConverter
            .ToString(entropyBytes)
            .Replace("-", "")
            .ToLower(), mnemonic.Language);
    }

    public string ConvertMnemonicToSeedHex(Mnemonic mnemonic, string password)
    {
        var mnemonicBytes = Encoding.UTF8.GetBytes(mnemonic.Value.Normalize(NormalizationForm.FormKD));
        var saltSuffix = password.IsNullOrEmpty() ? string.Empty : password;
        var salt = $"mnemonic{saltSuffix}";
        var saltBytes = Encoding.UTF8.GetBytes(salt);

        var rfc2898DerivedBytes = new Rfc2898DeriveBytes(mnemonicBytes, saltBytes, 2048, HashAlgorithmName.SHA512);
        var key = rfc2898DerivedBytes.GetBytes(64);
        var hex = BitConverter
            .ToString(key)
            .Replace("-", "")
            .ToLower();

        return hex;
    }

    public bool ValidateMnemonic(Mnemonic mnemonic)
    {
        try
        {
            ConvertMnemonicToEntropy(mnemonic);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public BlockchainWallet CreateWallet(int strength, Language language, string password)
    {   
        var mnemonic = GenerateMnemonic(strength, language);
        var seedHex = ConvertMnemonicToSeedHex(mnemonic, password);
        var masterWallet = new HDWallet<SDKWallet>(seedHex, "m/44'/1616'");
        var account = masterWallet.GetAccount(0);
        var wallet = account.GetExternalWallet(0);
        var keyPair = CryptoHelper.FromPrivateKey(wallet.PrivateKey);
        var privateKey = keyPair.PrivateKey.ToHex();
        var publicKey = keyPair.PublicKey.ToHex();
        var address = wallet.Address.ToBase58();
        var accountInfo = new Dictionary<string, string>();
        accountInfo.Add("Mnemonic", mnemonic.ToString());
        accountInfo.Add("PrivateKey", privateKey);
        accountInfo.Add("PublicKey", publicKey);
        accountInfo.Add("Address", address);
        return new BlockchainWallet(address, privateKey, mnemonic.ToString(), publicKey);
        }
    

    public BlockchainWallet GetWalletByMnemonic(string mnemonic, string password = null)
    {
        var accountInfo = new Dictionary<string, string>();
        var mnemonicValue = new Mnemonic
            {
                Value = mnemonic,
                Language = Language.English
            };
        var seedHex = ConvertMnemonicToSeedHex(mnemonicValue, password);
        var masterWallet = new HDWallet<SDKWallet>(seedHex, "m/44'/1616'");
        var account = masterWallet.GetAccount(0);
        var wallet = account.GetExternalWallet(0);
        var keyPair = CryptoHelper.FromPrivateKey(wallet.PrivateKey);
        var privateKey = keyPair.PrivateKey.ToHex();
        var publicKey = keyPair.PublicKey.ToHex();
        var address = wallet.Address.ToBase58();
        accountInfo.Add("Mnemonic", mnemonic);
        accountInfo.Add("PrivateKey", privateKey);
        accountInfo.Add("PublicKey", publicKey);
        accountInfo.Add("Address", address);
        
        return new BlockchainWallet(address, privateKey, mnemonic, publicKey);
    }

       // Convert hex string to byte array
    static byte[] StringToByteArray(string hexString)
    {
        int length = hexString.Length;
        byte[] byteArray = new byte[length / 2];

        for (int i = 0; i < length; i += 2)
        {
            byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }

        return byteArray;
    }
    public BlockchainWallet GetWalletByPrivateKey(string privateKey)
    {
        var accountInfo = new Dictionary<string, string>();
        var keybyte = StringToByteArray(privateKey);
        Array.Resize(ref keybyte, 32);
        var keyPair = CryptoHelper.FromPrivateKey(keybyte);
        var publicKey = keyPair.PublicKey.ToHex();
        accountInfo.Add("PrivateKey", privateKey);
        accountInfo.Add("PublicKey", publicKey);
        return new BlockchainWallet(null, privateKey, null, publicKey);
    }

}}