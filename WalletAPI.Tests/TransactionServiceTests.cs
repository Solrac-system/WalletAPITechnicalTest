using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WalletAPI.Application.Contracts.Persistence;
using WalletAPI.Application.DTOs;
using WalletAPI.Domain;
using WalletAPI.Infrastructure.Services;

namespace WalletAPI.Tests
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRespository> _transactionRepositoryMock;
        private readonly Mock<IWalletRespository> _walletRepositoryMock;
        private readonly TransactionService _transactionService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<TransactionService>> _loggerMock;

        public TransactionServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRespository>();
            _walletRepositoryMock = new Mock<IWalletRespository>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<TransactionService>>();
            _transactionService = new TransactionService(
                _transactionRepositoryMock.Object,
                _walletRepositoryMock.Object,
                mapperMock.Object,
                loggerMock.Object
            );

        }


        [Fact]
        public async Task CreateTransactionAsync_ShouldDebitBalance_WhenTransactionIsDebit()
        {
            // Arrange
            var transactionDto = new TransactionWriteDto
            {
                WalletId = 1, // Identificador de billetera válido
                Amount = 100.00m, // Monto válido
                Type = "Debit" // Tipo correcto
            };

            var wallet = new Wallet
            {
                Id = 1,
                Balance = 500.00m, // Saldo inicial suficiente
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var transaction = new Transaction
            {
                Id = 1,
                WalletId = transactionDto.WalletId,
                Amount = transactionDto.Amount,
                Type = transactionDto.Type,
                CreatedAt = DateTime.UtcNow
            };

            // Mock del repositorio de billetera
            _walletRepositoryMock.Setup(repo => repo.GetByIdAsync(transactionDto.WalletId))
                .ReturnsAsync(wallet);

            // Mock del mapper para Transaction (limpio y único)
            _mapperMock.Setup(mapper => mapper.Map<Transaction>(It.IsAny<TransactionWriteDto>()))
                .Returns(transaction);

            // Mock del mapper para TransactionReadDto
            _mapperMock.Setup(mapper => mapper.Map<TransactionReadDto>(It.IsAny<Transaction>()))
                .Returns(new TransactionReadDto
                {
                    Id = transaction.Id,
                    WalletId = transactionDto.WalletId,
                    Amount = transactionDto.Amount,
                    Type = transactionDto.Type,
                    CreatedAt = transaction.CreatedAt
                });

            // Validaciones antes del Act
            Assert.NotNull(transactionDto); // Aseguramos que DTO no es null
            Assert.NotNull(wallet);         // Confirmamos que Wallet no es null
            Assert.NotNull(transaction);    // Verificamos que Transaction no es null

            // Act
            var result = await _transactionService.CreateTransactionAsync(transactionDto);

            // Assert
            result.Should().NotBeNull(); // Verifica que se haya creado una transacción
            result.Amount.Should().Be(transactionDto.Amount); // Compara el monto
            wallet.Balance.Should().Be(400.00m); // Confirma que el saldo se actualizó correctamente
            _transactionRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Transaction>()), Times.Once); // Verifica la llamada al repositorio
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowArgumentException_WhenAmountIsNegative()
        {
            // Arrange
            var transactionDto = new TransactionWriteDto
            {
                WalletId = 1,
                Amount = -50.00m,
                Type = "Debit"
            };

            var service = new TransactionService(Mock.Of<ITransactionRespository>(), Mock.Of<IWalletRespository>(), Mock.Of<IMapper>(), Mock.Of<ILogger<TransactionService>>());

            // Act
            Func<Task> act = async () => await service.CreateTransactionAsync(transactionDto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>().WithMessage("Transaction amount must be greater than zero.");
        }


        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowInvalidOperationException_WhenInsufficientBalance()
        {
            // Arrange
            var wallet = new Wallet { Id = 1, Balance = 200.00m };
            var transactionDto = new TransactionWriteDto
            {
                WalletId = 1,
                Amount = 300.00m,
                Type = "Debit"
            };

            var walletRepoMock = new Mock<IWalletRespository>();
            walletRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(wallet);

            var service = new TransactionService(Mock.Of<ITransactionRespository>(), walletRepoMock.Object, Mock.Of<IMapper>(), Mock.Of<ILogger<TransactionService>>());

            // Act
            Func<Task> act = async () => await service.CreateTransactionAsync(transactionDto);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Insufficient balance.");
        }





    }
}
