using Xunit;
using BIP39Wallet;

namespace BIP39Wallet.Tests
{
    public class WalletTests
    {
        [Fact]
        public void CreateWallet_ReturnsValidAccountInfo()
        {
            // Arrange
            var wallet = new BIP39Wallet.Wallet();
            var strength = 128; // 设置助记词强度（以比特位为单位）
            var language = Language.English; // 设置助记词语言

            // Act
            var accountInfo = wallet.CreateWallet(strength, language, null);

            // Assert
            Assert.NotNull(accountInfo);
        }
    
        [Fact]
        public void GetWalletByMnemonic_ReturnsValidAccountInfo()
        {
            // Arrange
            var wallet = new Wallet();
            var mnemonic = "put draft unhappy diary arctic sponsor alien awesome adjust bubble maid brave";
            var accountInfo = wallet.GetWalletByMnemonic(mnemonic);
            Assert.NotNull(accountInfo);
            Assert.Equal("f0c3bf2cfc4f50405afb2f1236d653cf0581f4caedf4f1e0b49480c840659ba9", accountInfo.PrivateKey);
            Assert.Equal("04c0f6abf0e3122f4a49646d67bacf85c80ad726ca781ccba572033a31162f22e55a4a106760cbf1306f26c25aea1e4bb71ee66cb3c5104245d6040cce64546cc7", accountInfo.PublicKey);
        }

        [Fact]
        public void GetWalletByPrivateKey_ReturnsValidAccountInfo()
        {
            var wallet = new Wallet();
            var privateKey = "f0c3bf2cfc4f50405afb2f1236d653cf0581f4caedf4f1e0b49480c840659ba9";
            var accountInfo = wallet.GetWalletByPrivateKey(privateKey);
            Assert.NotNull(accountInfo);
            Assert.Equal("04c0f6abf0e3122f4a49646d67bacf85c80ad726ca781ccba572033a31162f22e55a4a106760cbf1306f26c25aea1e4bb71ee66cb3c5104245d6040cce64546cc7", accountInfo.PublicKey);
        }
    }
}
