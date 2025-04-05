using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Domain;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Tests
{
    public class WalletDbContextTests
    {
        private WalletDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            return new WalletDbContext(options);
        }

        [Fact]
        public async Task WalletDbContext_ShouldSaveAndRetrieveWallet()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var wallet = new Wallet
            {
                DocumentId = "123456",
                Name = "Test Wallet",
                Balance = 500.00m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            context.Wallet.Add(wallet);
            await context.SaveChangesAsync();

            var savedWallet = await context.Wallet.FirstOrDefaultAsync(w => w.DocumentId == "123456");

            // Assert
            savedWallet.Should().NotBeNull();
            savedWallet.Name.Should().Be("Test Wallet");
            savedWallet.Balance.Should().Be(500.00m);
        }


        [Fact]
        public async Task TransactionController_ShouldHandleTransactionCorrectly()
        {
            // Arrange: Usa el DbContext en memoria
            var context = new WalletDbContext(new DbContextOptionsBuilder<WalletDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options);

            var wallet = new Wallet
            {
                Id = 1,
                DocumentId = "123456",
                Name = "Test Wallet",
                Balance = 1000.00m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Wallet.Add(wallet);
            await context.SaveChangesAsync();

            var transaction = new Transaction
            {
                WalletId = 1,
                Amount = 200.00m,
                Type = "Debit",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            context.Transaction.Add(transaction);
            await context.SaveChangesAsync();

            var updatedWallet = await context.Wallet.FirstOrDefaultAsync(w => w.Id == 1);
            var transactions = await context.Transaction.Where(t => t.WalletId == 1).ToListAsync();

            // Assert
            updatedWallet.Balance.Should().Be(800.00m); // Verifica el saldo actualizado
            transactions.Should().HaveCount(1); // Verifica que la transacción fue guardada
        }


    }
}
