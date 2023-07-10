using Xunit;

namespace Wallet.Tests
{
    public class WalletTests
    {
        [Fact]
        public void CreateWallet_ReturnsValidWalletData()
        {
            // Arrange
            var wallet = new Wallet();
            // Act
            var walletData = wallet.CreateWallet(128, Language.English, "xibo123");
            // Assert
            Assert.NotNull(walletData);
        }
    }
}
